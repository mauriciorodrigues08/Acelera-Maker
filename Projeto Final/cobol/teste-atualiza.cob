       IDENTIFICATION DIVISION.
       PROGRAM-ID. TESTE-ATUALIZA.

       DATA DIVISION.
       WORKING-STORAGE SECTION.
       01 WS-CODIGO       PIC X(9) VALUE "000000001".
       01 WS-TELEFONE     PIC X(15) VALUE "11988887777".
       01 WS-EMAIL        PIC X(40) VALUE "joao.novo@teste.com".
       01 WS-STATUS       PIC X(2).

       PROCEDURE DIVISION.
           DISPLAY "Atualizando cliente codigo: " WS-CODIGO
           CALL "ATUALIZA_CLIENTE" USING WS-CODIGO WS-TELEFONE
                WS-EMAIL WS-STATUS
           END-CALL

           IF WS-STATUS = "00"
               DISPLAY "Atualizacao realizada com sucesso."
           ELSE
               IF WS-STATUS = "04"
                   DISPLAY "Cliente nao encontrado."
               ELSE
                   DISPLAY "Erro de conexao com o banco."
               END-IF
           END-IF
           STOP RUN.
