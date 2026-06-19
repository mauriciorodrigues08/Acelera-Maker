      ******************************************************************
      * COPYBOOK: TRANSACAO.CPY
      * DESCRICAO: LAYOUT DE ENTRADA DO ARQUIVO TRANSACOES.TXT
      * USO: INCLUDE PARA LEITURA DO ARQUIVO SEQUENCIAL DE TRANSACOES
      ******************************************************************
       01  REG-TRANSACAO.
           05  TRX-CLI-ID          PIC 9(05).
           05  TRX-ID              PIC 9(05).
           05  TRX-TIPO            PIC X(01).
           05  TRX-VALOR           PIC 9(09).
