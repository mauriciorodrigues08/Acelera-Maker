#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <sql.h>
#include <sqlext.h>

/* 
================================================================
FUNCOES DE I/O JSON (stdin/stdout)
================================================================
*/

/*
Utilitario: extrai valor de uma chave JSON simples (string ou numero)
Retorna 1 se encontrou, 0 se nao encontrou
*/
static int json_get_value(const char *json, const char *key,
                           char *out, int maxlen) {
    char search[64];
    snprintf(search, sizeof(search), "\"%s\"", key);
    const char *pos = strstr(json, search);
    if (!pos) return 0;
    pos += strlen(search);
    while (*pos == ' ' || *pos == ':' || *pos == ' ') pos++;
    int is_string = (*pos == '"');
    if (is_string) pos++;
    int i = 0;
    while (*pos && i < maxlen - 1) {
        if (is_string && *pos == '"') break;
        if (!is_string && (*pos == ',' || *pos == '}' || *pos == '\n')) break;
        out[i++] = *pos++;
    }
    out[i] = '\0';
    return 1;
}

/* 
LER_ENTRADA: le JSON do stdin e preenche campos COBOL
Saida:
  p_operacao  PIC X(1)   - "C" consulta, "A" atualiza
  p_codigo    PIC 9(9)   - codigo do cliente
  p_telefone  PIC X(15)  - novo telefone (so para operacao A)
  p_email     PIC X(60)  - novo email    (so para operacao A)
  p_status    PIC X(2)   - "00" ok, "08" erro de leitura
*/
void LER_ENTRADA(char *p_operacao, char *p_codigo, char *p_telefone,
                  char *p_email, char *p_status) {
    char buffer[512] = {0};
    char line[256];

    // le todas as linhas do stdin
    while (fgets(line, sizeof(line), stdin)) {
        strncat(buffer, line, sizeof(buffer) - strlen(buffer) - 1);
    }

    if (strlen(buffer) == 0) {
        memcpy(p_status, "08", 2);
        return;
    }

    // extrai operacao
    char op[4] = {0};
    if (!json_get_value(buffer, "operacao", op, sizeof(op))) {
        memcpy(p_status, "08", 2);
        return;
    }
    memset(p_operacao, ' ', 1);
    p_operacao[0] = op[0];

    // extrai codigo
    char cod[16] = {0};
    if (!json_get_value(buffer, "codigo", cod, sizeof(cod))) {
        memcpy(p_status, "08", 2);
        return;
    }
    memset(p_codigo, '0', 9);
    int cod_len = strlen(cod);
    if (cod_len > 9) cod_len = 9;
    memcpy(p_codigo + (9 - cod_len), cod, cod_len);

    // extrai telefone e email (opcionais, so para operacao A)
    char tel[16] = {0}, email[64] = {0};
    memset(p_telefone, ' ', 15);
    memset(p_email, ' ', 60);
    if (json_get_value(buffer, "telefone", tel, sizeof(tel)))
        memcpy(p_telefone, tel, strlen(tel) < 15 ? strlen(tel) : 15);
    if (json_get_value(buffer, "email", email, sizeof(email)))
        memcpy(p_email, email, strlen(email) < 60 ? strlen(email) : 60);

    memcpy(p_status, "00", 2);
}

/* 
ESCREVER_SAIDA: monta e escreve JSON no stdout
Entrada:
  p_status    PIC X(2)   - codigo de retorno
  p_mensagem  PIC X(100) - mensagem legivel
  p_codigo    PIC 9(9)   - codigo do cliente (opcional)
  p_nome      PIC X(50)  - nome             (opcional)
  p_telefone  PIC X(15)  - telefone         (opcional)
  p_email     PIC X(60)  - email            (opcional)
  p_incluir   PIC X(1)   - "S" inclui dados do cliente, "N" so status
*/
void ESCREVER_SAIDA(char *p_status, char *p_mensagem, char *p_codigo,
                     char *p_nome, char *p_telefone, char *p_email,
                     char *p_incluir) {

    // copia e termina strings COBOL (remove espacos a direita)
    char status[3], mensagem[101], nome[51], tel[16], email[61], cod[10];

    memcpy(status, p_status, 2); status[2] = '\0';
    memcpy(mensagem, p_mensagem, 100); mensagem[100] = '\0';
    for (int i = 99; i >= 0 && mensagem[i] == ' '; i--) mensagem[i] = '\0';

    memcpy(cod, p_codigo, 9); cod[9] = '\0';
    memcpy(nome, p_nome, 50); nome[50] = '\0';
    for (int i = 49; i >= 0 && nome[i] == ' '; i--) nome[i] = '\0';
    memcpy(tel, p_telefone, 15); tel[15] = '\0';
    for (int i = 14; i >= 0 && tel[i] == ' '; i--) tel[i] = '\0';
    memcpy(email, p_email, 60); email[60] = '\0';
    for (int i = 59; i >= 0 && email[i] == ' '; i--) email[i] = '\0';

    if (p_incluir[0] == 'S') {
        // converte codigo de texto para numero (remove zeros a esquerda)
        int codigo_int = atoi(cod);
        printf("{\n");
        printf("  \"status\": \"%s\",\n", status);
        printf("  \"mensagem\": \"%s\",\n", mensagem);
        printf("  \"codigo\": %d,\n", codigo_int);
        printf("  \"nome\": \"%s\",\n", nome);
        printf("  \"telefone\": \"%s\",\n", tel);
        printf("  \"email\": \"%s\"\n", email);
        printf("}\n");
    } else {
        printf("{\n");
        printf("  \"status\": \"%s\",\n", status);
        printf("  \"mensagem\": \"%s\"\n", mensagem);
        printf("}\n");
    }
    fflush(stdout);
}

/* 
================================================================
FUNCOES DE ACESSO AO BANCO (SQLite via ODBC)
================================================================
*/
void CONSULTA_CLIENTE(char *p_codigo, char *p_nome, char *p_telefone,
                       char *p_email, char *p_status) {
    SQLHENV henv; SQLHDBC hdbc; SQLHSTMT hstmt; SQLRETURN ret;
    char codigo_str[16];
    memcpy(codigo_str, p_codigo, 9); codigo_str[9] = '\0';

    SQLAllocHandle(SQL_HANDLE_ENV, SQL_NULL_HANDLE, &henv);
    SQLSetEnvAttr(henv, SQL_ATTR_ODBC_VERSION, (void*)SQL_OV_ODBC3, 0);
    SQLAllocHandle(SQL_HANDLE_DBC, henv, &hdbc);

    ret = SQLConnect(hdbc, (SQLCHAR*)"clientesDB", SQL_NTS, NULL, 0, NULL, 0);
    if (!SQL_SUCCEEDED(ret)) {
        memcpy(p_status, "08", 2);
        SQLFreeHandle(SQL_HANDLE_DBC, hdbc);
        SQLFreeHandle(SQL_HANDLE_ENV, henv);
        return;
    }

    SQLAllocHandle(SQL_HANDLE_STMT, hdbc, &hstmt);
    char query[200];
    snprintf(query, sizeof(query),
        "SELECT nome, telefone, email FROM clientes WHERE codigo = %s",
        codigo_str);

    ret = SQLExecDirect(hstmt, (SQLCHAR*)query, SQL_NTS);
    if (SQL_SUCCEEDED(ret)) {
        ret = SQLFetch(hstmt);
        if (ret == SQL_SUCCESS || ret == SQL_SUCCESS_WITH_INFO) {
            char nome[64]={0}, tel[64]={0}, email[64]={0};
            SQLLEN ind;
            SQLGetData(hstmt, 1, SQL_C_CHAR, nome, sizeof(nome), &ind);
            SQLGetData(hstmt, 2, SQL_C_CHAR, tel,  sizeof(tel),  &ind);
            SQLGetData(hstmt, 3, SQL_C_CHAR, email,sizeof(email),&ind);
            memset(p_nome,     ' ', 50); memcpy(p_nome,     nome,  strlen(nome)  < 50 ? strlen(nome)  : 50);
            memset(p_telefone, ' ', 15); memcpy(p_telefone, tel,   strlen(tel)   < 15 ? strlen(tel)   : 15);
            memset(p_email,    ' ', 60); memcpy(p_email,    email, strlen(email) < 60 ? strlen(email) : 60);
            memcpy(p_status, "00", 2);
        } else {
            memcpy(p_status, "04", 2);
        }
    } else {
        memcpy(p_status, "08", 2);
    }

    SQLFreeHandle(SQL_HANDLE_STMT, hstmt);
    SQLDisconnect(hdbc);
    SQLFreeHandle(SQL_HANDLE_DBC, hdbc);
    SQLFreeHandle(SQL_HANDLE_ENV, henv);
}

void ATUALIZA_CLIENTE(char *p_codigo, char *p_telefone, char *p_email,
                       char *p_status) {
    SQLHENV henv; SQLHDBC hdbc; SQLHSTMT hstmt; SQLRETURN ret;
    SQLLEN rowcount = 0;

    char codigo_str[16];
    memcpy(codigo_str, p_codigo, 9); codigo_str[9] = '\0';

    char tel_str[16];
    memcpy(tel_str, p_telefone, 15); tel_str[15] = '\0';
    for (int i = 14; i >= 0 && tel_str[i] == ' '; i--) tel_str[i] = '\0';

    char email_str[61];
    memcpy(email_str, p_email, 60); email_str[60] = '\0';
    for (int i = 59; i >= 0 && email_str[i] == ' '; i--) email_str[i] = '\0';

    SQLAllocHandle(SQL_HANDLE_ENV, SQL_NULL_HANDLE, &henv);
    SQLSetEnvAttr(henv, SQL_ATTR_ODBC_VERSION, (void*)SQL_OV_ODBC3, 0);
    SQLAllocHandle(SQL_HANDLE_DBC, henv, &hdbc);

    ret = SQLConnect(hdbc, (SQLCHAR*)"clientesDB", SQL_NTS, NULL, 0, NULL, 0);
    if (!SQL_SUCCEEDED(ret)) {
        memcpy(p_status, "08", 2);
        SQLFreeHandle(SQL_HANDLE_DBC, hdbc);
        SQLFreeHandle(SQL_HANDLE_ENV, henv);
        return;
    }

    SQLSetConnectAttr(hdbc, SQL_ATTR_AUTOCOMMIT,
                      (void*)SQL_AUTOCOMMIT_OFF, 0);
    SQLAllocHandle(SQL_HANDLE_STMT, hdbc, &hstmt);

    char query[300];
    snprintf(query, sizeof(query),
        "UPDATE clientes SET telefone='%s', email='%s' WHERE codigo=%s",
        tel_str, email_str, codigo_str);

    ret = SQLExecDirect(hstmt, (SQLCHAR*)query, SQL_NTS);
    if (SQL_SUCCEEDED(ret)) {
        SQLRowCount(hstmt, &rowcount);
        if (rowcount > 0) {
            SQLEndTran(SQL_HANDLE_DBC, hdbc, SQL_COMMIT);
            memcpy(p_status, "00", 2);
        } else {
            SQLEndTran(SQL_HANDLE_DBC, hdbc, SQL_ROLLBACK);
            memcpy(p_status, "04", 2);
        }
    } else {
        SQLEndTran(SQL_HANDLE_DBC, hdbc, SQL_ROLLBACK);
        memcpy(p_status, "08", 2);
    }

    SQLFreeHandle(SQL_HANDLE_STMT, hstmt);
    SQLDisconnect(hdbc);
    SQLFreeHandle(SQL_HANDLE_DBC, hdbc);
    SQLFreeHandle(SQL_HANDLE_ENV, henv);
}