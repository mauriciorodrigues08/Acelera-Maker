       IDENTIFICATION DIVISION.
       PROGRAM-ID. CLIENTES.
       *> --------------------------------------------------------------
       *> Programa principal do sistema de cadastro de clientes.
       *> Le uma operacao em JSON via stdin, executa via rotinas ODBC
       *> e escreve a resposta em JSON via stdout.
       *>
       *> Operacoes suportadas:
       *>   "C" - Consultar cliente por codigo
       *>   "A" - Atualizar telefone e email do cliente
       *>
       *> Codigos de status de retorno:
       *>   "00" - Sucesso
       *>   "04" - Cliente nao encontrado
       *>   "08" - Erro interno
       *> --------------------------------------------------------------
       
       DATA DIVISION.
       WORKING-STORAGE SECTION.
       
       *> Campos de controle da operacao
       01 WS-OPERACAO         PIC X(1).
       88 OP-CONSULTA      VALUE 'C'.
       88 OP-ATUALIZA      VALUE 'A'.
       
       01 WS-STATUS           PIC X(2).
       88 STATUS-OK        VALUE '00'.
       88 STATUS-NAO-FOUND VALUE '04'.
       88 STATUS-ERRO      VALUE '08'.
       
       01 WS-MENSAGEM         PIC X(100).
       01 WS-INCLUIR-DADOS    PIC X(1).
       
       *> Campos do cliente
       01 WS-CODIGO           PIC 9(9).
       01 WS-NOME             PIC X(50).
       01 WS-TELEFONE         PIC X(15).
       01 WS-EMAIL            PIC X(60).
       
       *> Campos auxiliares para entrada
       01 WS-CODIGO-ENTRADA   PIC X(9).
       01 WS-STATUS-LEITURA   PIC X(2).
       
       PROCEDURE DIVISION.
           *> ----------------------------------------------------------
           *> 1. Ler e interpretar a entrada JSON do stdin
           *> ----------------------------------------------------------
           CALL 'LER_ENTRADA' USING WS-OPERACAO
                                    WS-CODIGO-ENTRADA
                                    WS-TELEFONE
                                    WS-EMAIL
                                    WS-STATUS-LEITURA
           END-CALL
           
           IF WS-STATUS-LEITURA NOT = '00'
                   MOVE '08' TO WS-STATUS
                   MOVE 'Erro ao ler entrada JSON.' TO WS-MENSAGEM
                   MOVE 'N' TO WS-INCLUIR-DADOS
                   PERFORM ESCREVER-E-SAIR
           END-IF
           
           MOVE FUNCTION NUMVAL(WS-CODIGO-ENTRADA) TO WS-CODIGO
           
           *> ----------------------------------------------------------
           *> 2. Executar a operacao solicitada
           *> ----------------------------------------------------------
           EVALUATE TRUE
                   WHEN OP-CONSULTA
                        PERFORM EXECUTAR-CONSULTA
                   
                   WHEN OP-ATUALIZA
                        PERFORM EXECUTAR-ATUALIZA
                   
                   WHEN OTHER
                        MOVE '08'
                            TO WS-STATUS
                        MOVE 'Operacao invalida. Use C ou A.'
                            TO WS-MENSAGEM
                        MOVE 'N'
                            TO WS-INCLUIR-DADOS
                        PERFORM ESCREVER-E-SAIR
           END-EVALUATE
           
           *> ----------------------------------------------------------
           *> 3. Escrever resposta JSON no stdout
           *> ----------------------------------------------------------
           CALL 'ESCREVER_SAIDA' USING WS-STATUS
                                       WS-MENSAGEM
                                       WS-CODIGO-ENTRADA
                                       WS-NOME
                                       WS-TELEFONE
                                       WS-EMAIL
                                       WS-INCLUIR-DADOS
           END-CALL
           STOP RUN.
           
       *> --------------------------------------------------------------
       *> EXECUTAR-CONSULTA: busca cliente pelo codigo
       *> --------------------------------------------------------------
       EXECUTAR-CONSULTA.
           INITIALIZE WS-NOME WS-TELEFONE WS-EMAIL
           
           CALL 'CONSULTA_CLIENTE' USING WS-CODIGO-ENTRADA
                                         WS-NOME
                                         WS-TELEFONE
                                         WS-EMAIL
                                         WS-STATUS
           END-CALL
           
           EVALUATE TRUE
                   WHEN STATUS-OK
                        MOVE 'Cliente encontrado.' TO WS-MENSAGEM
                        MOVE 'S' TO WS-INCLUIR-DADOS
                   
                   WHEN STATUS-NAO-FOUND
                        MOVE 'Cliente nao encontrado.' TO WS-MENSAGEM
                        MOVE 'N' TO WS-INCLUIR-DADOS
                   
                   WHEN OTHER
                        MOVE 'Erro ao consultar o banco de dados.'
                            TO WS-MENSAGEM
                        MOVE 'N' TO WS-INCLUIR-DADOS
           END-EVALUATE.
           
       *> --------------------------------------------------------------
       *> EXECUTAR-ATUALIZA: atualiza telefone e email do cliente
       *> --------------------------------------------------------------
       EXECUTAR-ATUALIZA.
           CALL 'ATUALIZA_CLIENTE' USING WS-CODIGO-ENTRADA
                                         WS-TELEFONE
                                         WS-EMAIL
                                         WS-STATUS
           END-CALL
           
           EVALUATE TRUE
                   WHEN STATUS-OK
                        MOVE 'Dados atualizados com sucesso.'
                            TO WS-MENSAGEM
                        MOVE 'N'
                            TO WS-INCLUIR-DADOS
                            
                   WHEN STATUS-NAO-FOUND
                        MOVE 'Cliente nao encontrado.'
                            TO WS-MENSAGEM
                        MOVE 'N'
                            TO WS-INCLUIR-DADOS

                   WHEN OTHER
                        MOVE 'Erro ao atualizar o banco de dados.'
                            TO WS-MENSAGEM
                        MOVE 'N'
                            TO WS-INCLUIR-DADOS
           END-EVALUATE.
           
       *> --------------------------------------------------------------
       *> ESCREVER-E-SAIR: escreve resposta de erro e encerra
       *> --------------------------------------------------------------
       ESCREVER-E-SAIR.
           INITIALIZE WS-CODIGO-ENTRADA WS-NOME WS-TELEFONE WS-EMAIL
           CALL 'ESCREVER_SAIDA' USING WS-STATUS
                                            WS-MENSAGEM
                                            WS-CODIGO-ENTRADA
                                            WS-NOME
                                            WS-TELEFONE
                                            WS-EMAIL
                                            WS-INCLUIR-DADOS
           END-CALL
           STOP RUN.
