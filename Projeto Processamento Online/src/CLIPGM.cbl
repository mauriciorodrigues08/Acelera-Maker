      *----------------------------------------------------------------*
      * CLIPGM.CBL - VERSAO EXECUTAVEL (GnuCOBOL)                      *
      *                                                                *
      * Simula no terminal o comportamento da transacao CICS CLIE,     *
      * que executaria o programa CLIPGM em ambiente mainframe real.   *
      *                                                                *
      * O GnuCOBOL deste ambiente foi compilado SEM suporte a arquivo  *
      * indexado (ISAM/VBISAM/BDB) - comum em instalacoes padrao do    *
      * open-cobol. Por isso o "VSAM CLIENTES" do enunciado e          *
      * representado por um arquivo sequencial (data/clientes.dat,     *
      * gerado pelo CARGA.cbl), carregado em uma TABELA EM MEMORIA     *
      * no inicio do programa. O acesso por chave (READ/REWRITE do     *
      * CICS) e simulado por busca/atualizacao nessa tabela.           *
      *                                                                *
      * Equivalencias com o ambiente CICS real (ver CLIPGM_CICS.cbl    *
      * e docs/README.md para a versao "de papel" fiel ao CICS):       *
      *                                                                *
      *   Conceito CICS                  | Simulado aqui como          *
      *   --------------------------------------------------------     *
      *   EXEC CICS SEND MAP             | PERFORM 1000-EXIBIR-TELA    *
      *   EXEC CICS RECEIVE MAP          | ACCEPT da opcao/PF          *
      *   EXEC CICS READ FILE            | Busca na tabela em memoria  *
      *   EXEC CICS REWRITE FILE         | Atualiza a tabela em        *
      *                                  | memoria (persistida em      *
      *                                  | disco so na saida - PF3)    *
      *   EXEC CICS RETURN COMMAREA      | WS-POS-ENCONTRADA mantido   *
      *                                  | entre as opcoes do loop -   *
      *                                  | "lembra" qual cliente esta  *
      *                                  | carregado na tela           *
      *   EXEC CICS XCTL                 | Nao se aplica aqui (ver     *
      *                                  | CLIPGM_CICS.cbl para uma    *
      *                                  | demonstracao do comando)    *
      *----------------------------------------------------------------*
       IDENTIFICATION DIVISION.
       PROGRAM-ID. CLIPGM.

       ENVIRONMENT DIVISION.
       INPUT-OUTPUT SECTION.
       FILE-CONTROL.
           SELECT ARQ-CLIENTES ASSIGN TO "data/clientes.dat"
               ORGANIZATION IS LINE SEQUENTIAL
               FILE STATUS IS WS-FILE-STATUS.

       DATA DIVISION.
       FILE SECTION.
       FD  ARQ-CLIENTES.
       01  REG-CLIENTE.
           05  REC-CODCLI          PIC 9(6).
           05  REC-NOME            PIC X(30).
           05  REC-TELEFONE        PIC X(15).
           05  REC-CIDADE          PIC X(20).

       WORKING-STORAGE SECTION.
       01  WS-FILE-STATUS          PIC XX.
       01  WS-OPCAO                PIC X.
       01  WS-CONTINUA             PIC X VALUE "S".
       01  WS-FIM-ARQUIVO          PIC X VALUE "N".

      *--------------------------------------------------------------*
      * Tabela em memoria - representa o arquivo CLIENTES (VSAM)     *
      *--------------------------------------------------------------*
       01  WS-QTD-CLIENTES         PIC 9(3) VALUE ZERO.
       01  TAB-CLIENTES.
           05  TAB-CLIENTE OCCURS 50 TIMES.
               10  TAB-CODCLI      PIC 9(6).
               10  TAB-NOME        PIC X(30).
               10  TAB-TELEFONE    PIC X(15).
               10  TAB-CIDADE      PIC X(20).
       01  WS-IDX                  PIC 9(3) VALUE ZERO.

      *--------------------------------------------------------------*
      * Equivalente, em espirito, a DFHCOMMAREA: guarda a POSICAO    *
      * na tabela do cliente atualmente "em tela" (carregado pelo    *
      * PF5), para que o PF6 saiba o que atualizar sem precisar      *
      * buscar de novo. Persiste entre as opcoes digitadas dentro    *
      * da mesma execucao do programa.                                *
      *--------------------------------------------------------------*
       01  WS-POS-ENCONTRADA       PIC 9(3) VALUE ZERO.

       01  TELA-CLIENTE.
           05  TELA-CODCLI         PIC 9(6) VALUE ZERO.
           05  TELA-NOME           PIC X(30) VALUE SPACES.
           05  TELA-TELEFONE       PIC X(15) VALUE SPACES.
           05  TELA-CIDADE         PIC X(20) VALUE SPACES.

      *--------------------------------------------------------------*
      * Campos auxiliares para capturar o que o usuario digitou no   *
      * PF6, antes de decidir se mantem o valor atual (Enter em      *
      * branco) ou aplica o novo valor digitado.                     *
      *--------------------------------------------------------------*
       01  WS-TELEFONE-DIGITADO    PIC X(15) VALUE SPACES.
       01  WS-CIDADE-DIGITADA      PIC X(20) VALUE SPACES.

       01  TELA-MENSAGEM           PIC X(30) VALUE SPACES.

      *--------------------------------------------------------------*
      * Mensagens fixas do sistema - iguais as definidas no README   *
      *--------------------------------------------------------------*
       01  WS-MENSAGENS.
           05  MSG-ENCONTRADO      PIC X(30)
                                   VALUE "CLIENTE ENCONTRADO".
           05  MSG-NAO-ENCONTRADO  PIC X(30)
                                   VALUE "CLIENTE NAO ENCONTRADO".
           05  MSG-ALTERADO        PIC X(30)
                                   VALUE "ALTERACAO REALIZADA".
           05  MSG-CAMPO-OBRIG     PIC X(30)
                                   VALUE "CODIGO OBRIGATORIO".
           05  MSG-SEM-CONSULTA    PIC X(30)
                                   VALUE "CONSULTE ANTES DE SALVAR".
           05  MSG-OPCAO-INVALIDA  PIC X(30)
                                   VALUE "OPCAO INVALIDA".

       PROCEDURE DIVISION.
       0000-PRINCIPAL.
           PERFORM 0500-CARREGAR-TABELA

           PERFORM UNTIL WS-CONTINUA = "N"
               PERFORM 1000-EXIBIR-TELA
               PERFORM 2000-LER-OPCAO

               EVALUATE WS-OPCAO
                   WHEN "5"
                       PERFORM 3000-CONSULTAR
                   WHEN "6"
                       PERFORM 4000-SALVAR
                   WHEN "3"
                       MOVE "N" TO WS-CONTINUA
                   WHEN OTHER
                       MOVE MSG-OPCAO-INVALIDA TO TELA-MENSAGEM
               END-EVALUATE
           END-PERFORM

           PERFORM 0600-GRAVAR-TABELA

           DISPLAY " "
           DISPLAY "Transacao CLIE encerrada (PF3)."
           STOP RUN.

      *--------------------------------------------------------------*
      * Carrega o arquivo sequencial inteiro para a tabela em        *
      * memoria, no inicio do programa.                               *
      *--------------------------------------------------------------*
       0500-CARREGAR-TABELA.
           OPEN INPUT ARQ-CLIENTES

           IF WS-FILE-STATUS NOT = "00"
              DISPLAY " "
              DISPLAY "Erro ao abrir data/clientes.dat - STATUS: "
                      WS-FILE-STATUS
              DISPLAY "Execute o programa CARGA antes de continuar."
              STOP RUN
           END-IF

           PERFORM UNTIL WS-FIM-ARQUIVO = "S"
               READ ARQ-CLIENTES
                   AT END
                       MOVE "S" TO WS-FIM-ARQUIVO
                   NOT AT END
                       ADD 1 TO WS-QTD-CLIENTES
                       MOVE REC-CODCLI
                            TO TAB-CODCLI(WS-QTD-CLIENTES)
                       MOVE REC-NOME
                            TO TAB-NOME(WS-QTD-CLIENTES)
                       MOVE REC-TELEFONE
                            TO TAB-TELEFONE(WS-QTD-CLIENTES)
                       MOVE REC-CIDADE
                            TO TAB-CIDADE(WS-QTD-CLIENTES)
               END-READ
           END-PERFORM

           CLOSE ARQ-CLIENTES.

      *--------------------------------------------------------------*
      * Persiste a tabela (com as alteracoes do PF6) de volta no     *
      * arquivo, ao encerrar o programa (PF3).                       *
      *--------------------------------------------------------------*
       0600-GRAVAR-TABELA.
           OPEN OUTPUT ARQ-CLIENTES

           PERFORM VARYING WS-IDX FROM 1 BY 1
                   UNTIL WS-IDX > WS-QTD-CLIENTES
               MOVE TAB-CODCLI(WS-IDX)     TO REC-CODCLI
               MOVE TAB-NOME(WS-IDX)       TO REC-NOME
               MOVE TAB-TELEFONE(WS-IDX)   TO REC-TELEFONE
               MOVE TAB-CIDADE(WS-IDX)     TO REC-CIDADE
               WRITE REG-CLIENTE
           END-PERFORM

           CLOSE ARQ-CLIENTES.

      *--------------------------------------------------------------*
      * Equivalente ao EXEC CICS SEND MAP - exibe a tela atual       *
      *--------------------------------------------------------------*
       1000-EXIBIR-TELA.
           DISPLAY " ".
           DISPLAY "****************************************".
           DISPLAY "* CONSULTA DE CLIENTES                 *".
           DISPLAY "****************************************".
           DISPLAY " ".
           DISPLAY "Codigo Cliente: " TELA-CODCLI.
           DISPLAY "Nome.........: " TELA-NOME.
           DISPLAY "Telefone......: " TELA-TELEFONE.
           DISPLAY "Cidade........: " TELA-CIDADE.
           DISPLAY "Mensagem......: " TELA-MENSAGEM.
           DISPLAY " ".
           DISPLAY "PF3=Sair    PF5=Consultar    PF6=Salvar".

      *--------------------------------------------------------------*
      * Equivalente ao EXEC CICS RECEIVE MAP - le a tecla de funcao  *
      *--------------------------------------------------------------*
       2000-LER-OPCAO.
           MOVE SPACES TO TELA-MENSAGEM
           DISPLAY " "
           DISPLAY "Opcao (5=Consultar  6=Salvar  3=Sair): "
                   WITH NO ADVANCING
           ACCEPT WS-OPCAO.

      *--------------------------------------------------------------*
      * PF5 - Equivalente ao EXEC CICS READ FILE('CLIENTES')         *
      *--------------------------------------------------------------*
       3000-CONSULTAR.
           DISPLAY "Informe o codigo do cliente: " WITH NO ADVANCING
           ACCEPT TELA-CODCLI

           IF TELA-CODCLI = ZERO
              MOVE MSG-CAMPO-OBRIG TO TELA-MENSAGEM
           ELSE
              MOVE ZERO TO WS-POS-ENCONTRADA
              PERFORM VARYING WS-IDX FROM 1 BY 1
                      UNTIL WS-IDX > WS-QTD-CLIENTES
                  IF TAB-CODCLI(WS-IDX) = TELA-CODCLI
                     MOVE WS-IDX TO WS-POS-ENCONTRADA
                  END-IF
              END-PERFORM

              IF WS-POS-ENCONTRADA > ZERO
                 MOVE TAB-NOME(WS-POS-ENCONTRADA)
                      TO TELA-NOME
                 MOVE TAB-TELEFONE(WS-POS-ENCONTRADA)
                      TO TELA-TELEFONE
                 MOVE TAB-CIDADE(WS-POS-ENCONTRADA)
                      TO TELA-CIDADE
                 MOVE MSG-ENCONTRADO TO TELA-MENSAGEM
              ELSE
                 MOVE SPACES TO TELA-NOME
                                 TELA-TELEFONE
                                 TELA-CIDADE
                 MOVE MSG-NAO-ENCONTRADO TO TELA-MENSAGEM
              END-IF
           END-IF.

      *--------------------------------------------------------------*
      * PF6 - Equivalente ao EXEC CICS READ UPDATE + REWRITE          *
      * Atualiza apenas TELEFONE e CIDADE, conforme regra do projeto *
      * Usa o cliente ja carregado pelo PF5 (WS-POS-ENCONTRADA),      *
      * exatamente como o CICS faria via COMMAREA.                    *
      *--------------------------------------------------------------*
       4000-SALVAR.
           IF WS-POS-ENCONTRADA = ZERO
              MOVE MSG-SEM-CONSULTA TO TELA-MENSAGEM
           ELSE
              MOVE TELA-TELEFONE TO WS-TELEFONE-DIGITADO
              MOVE TELA-CIDADE   TO WS-CIDADE-DIGITADA

              DISPLAY "Novo telefone (" TELA-TELEFONE
                      ") [ENTER mantem]: " WITH NO ADVANCING
              ACCEPT WS-TELEFONE-DIGITADO

              DISPLAY "Nova cidade...(" TELA-CIDADE
                      ") [ENTER mantem]: " WITH NO ADVANCING
              ACCEPT WS-CIDADE-DIGITADA

      *       Enter em branco (ACCEPT zera o campo com SPACES) deve
      *       MANTER o valor atual, nao apagar o dado do cliente.
              IF WS-TELEFONE-DIGITADO NOT = SPACES
                 MOVE WS-TELEFONE-DIGITADO TO TELA-TELEFONE
              END-IF

              IF WS-CIDADE-DIGITADA NOT = SPACES
                 MOVE WS-CIDADE-DIGITADA TO TELA-CIDADE
              END-IF

              MOVE TELA-TELEFONE
                   TO TAB-TELEFONE(WS-POS-ENCONTRADA)
              MOVE TELA-CIDADE
                   TO TAB-CIDADE(WS-POS-ENCONTRADA)

              MOVE MSG-ALTERADO TO TELA-MENSAGEM
           END-IF.
