#include <stdio.h>
#include <string.h>
#include <sql.h>
#include <sqlext.h>

/* Funcao chamada pelo COBOL: consulta cliente por codigo
   Parametros (todos passados por referencia, como strings COBOL):
   p_codigo   (entrada, 9 digitos texto)
   p_nome     (saida, 30 chars)
   p_telefone (saida, 15 chars)
   p_email    (saida, 40 chars)
   p_status   (saida, 2 chars: "00"=ok, "04"=nao encontrado, "08"=erro)
*/
void CONSULTA_CLIENTE(char *p_codigo, char *p_nome, char *p_telefone,
                       char *p_email, char *p_status) {
    SQLHENV henv;
    SQLHDBC hdbc;
    SQLHSTMT hstmt;
    SQLRETURN ret;

    char codigo_str[16];
    memcpy(codigo_str, p_codigo, 9);
    codigo_str[9] = '\0';

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
        "SELECT nome, telefone, email FROM clientes WHERE codigo = %s", codigo_str);

    ret = SQLExecDirect(hstmt, (SQLCHAR*)query, SQL_NTS);
    if (SQL_SUCCEEDED(ret)) {
        ret = SQLFetch(hstmt);
        if (ret == SQL_SUCCESS || ret == SQL_SUCCESS_WITH_INFO) {
            char nome[64]={0}, tel[64]={0}, email[64]={0};
            SQLLEN ind;
            SQLGetData(hstmt, 1, SQL_C_CHAR, nome, sizeof(nome), &ind);
            SQLGetData(hstmt, 2, SQL_C_CHAR, tel, sizeof(tel), &ind);
            SQLGetData(hstmt, 3, SQL_C_CHAR, email, sizeof(email), &ind);

            memset(p_nome, ' ', 30); memcpy(p_nome, nome, strlen(nome) < 30 ? strlen(nome) : 30);
            memset(p_telefone, ' ', 15); memcpy(p_telefone, tel, strlen(tel) < 15 ? strlen(tel) : 15);
            memset(p_email, ' ', 40); memcpy(p_email, email, strlen(email) < 40 ? strlen(email) : 40);
            memcpy(p_status, "00", 2);
        } else {
            memcpy(p_status, "04", 2); /* nao encontrado */
        }
    } else {
        memcpy(p_status, "08", 2);
    }

    SQLFreeHandle(SQL_HANDLE_STMT, hstmt);
    SQLDisconnect(hdbc);
    SQLFreeHandle(SQL_HANDLE_DBC, hdbc);
    SQLFreeHandle(SQL_HANDLE_ENV, henv);
}

/* Funcao chamada pelo COBOL: atualiza telefone e email de um cliente
   Parametros:
   p_codigo   (entrada, 9 digitos texto)
   p_telefone (entrada, 15 chars)
   p_email    (entrada, 40 chars)
   p_status   (saida, 2 chars: "00"=ok, "04"=nao encontrado, "08"=erro)
*/
void ATUALIZA_CLIENTE(char *p_codigo, char *p_telefone, char *p_email,
                       char *p_status) {
    SQLHENV henv;
    SQLHDBC hdbc;
    SQLHSTMT hstmt;
    SQLRETURN ret;
    SQLLEN rowcount = 0;

    char codigo_str[16];
    memcpy(codigo_str, p_codigo, 9);
    codigo_str[9] = '\0';

    char tel_str[16];
    memcpy(tel_str, p_telefone, 15);
    tel_str[15] = '\0';
    /* remove espacos a direita */
    for (int i = 14; i >= 0 && tel_str[i] == ' '; i--) tel_str[i] = '\0';

    char email_str[41];
    memcpy(email_str, p_email, 40);
    email_str[40] = '\0';
    for (int i = 39; i >= 0 && email_str[i] == ' '; i--) email_str[i] = '\0';

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

    /* desativa autocommit para controlar a transacao manualmente */
    SQLSetConnectAttr(hdbc, SQL_ATTR_AUTOCOMMIT, (void*)SQL_AUTOCOMMIT_OFF, 0);

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
            memcpy(p_status, "04", 2); /* nao encontrado */
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
