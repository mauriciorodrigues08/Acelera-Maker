       IDENTIFICATION DIVISION.
       PROGRAM-ID. TESTE-CONSULTA.

       DATA DIVISION.
       WORKING-STORAGE SECTION.
       01 WS-CODIGO       PIC X(9) VALUE "000000001".
       01 WS-NOME         PIC X(30).
       01 WS-TELEFONE     PIC X(15).
       01 WS-EMAIL        PIC X(40).
       01 WS-STATUS       PIC X(2).

       PROCEDURE DIVISION.
           DISPLAY "Consultando cliente codigo: " WS-CODIGO
           CALL "CONSULTA_CLIENTE" USING WS-CODIGO WS-NOME
                WS-TELEFONE WS-EMAIL WS-STATUS
           END-CALL

           IF WS-STATUS = "00"
               DISPLAY "Nome.....: " WS-NOME
               DISPLAY "Telefone.: " WS-TELEFONE
               DISPLAY "Email....: " WS-EMAIL
           ELSE
               IF WS-STATUS = "04"
                   DISPLAY "Cliente nao encontrado."
               ELSE
                   DISPLAY "Erro de conexao com o banco."
               END-IF
           END-IF
           STOP RUN.
