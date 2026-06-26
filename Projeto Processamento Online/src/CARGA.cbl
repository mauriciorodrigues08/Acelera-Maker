      *----------------------------------------------------------------*
      * CARGA.CBL                                                      *
      * Programa utilitario - NAO faz parte do entregavel CICS.        *
      * Cria o arquivo data/clientes.dat (LINE SEQUENTIAL, registro     *
      * fixo de 71 colunas) com a massa de dados de exemplo.            *
      *                                                                *
      * Por que SEQUENTIAL e nao INDEXED (VSAM/ISAM)?                  *
      * O pacote GnuCOBOL usado neste ambiente foi compilado SEM        *
      * suporte a arquivo indexado (ISAM/VBISAM/BDB). Isso e comum em  *
      * instalacoes padrao do open-cobol. Para nao depender de         *
      * bibliotecas extras, o "VSAM CLIENTES" do enunciado e           *
      * representado por um arquivo sequencial, carregado em uma       *
      * TABELA EM MEMORIA pelo CLIPGM.cbl - que e quem de fato          *
      * simula o acesso por chave (READ/REWRITE).                      *
      *                                                                *
      * Executar UMA VEZ antes de rodar o CLIPGM.cbl.                  *
      *----------------------------------------------------------------*
       IDENTIFICATION DIVISION.
       PROGRAM-ID. CARGA.

       ENVIRONMENT DIVISION.
       INPUT-OUTPUT SECTION.
       FILE-CONTROL.
           SELECT ARQ-CLIENTES ASSIGN TO "data/clientes.dat"
               ORGANIZATION IS LINE SEQUENTIAL
               FILE STATUS IS WS-FILE-STATUS.

       DATA DIVISION.
       FILE SECTION.
       FD  ARQ-CLIENTES.
       01  REG-LINHA               PIC X(71).

       WORKING-STORAGE SECTION.
       01  WS-FILE-STATUS          PIC XX.
       01  WS-TOTAL-CARGA          PIC 9(3) VALUE ZERO.

      *--------------------------------------------------------------*
      * Layout do registro (igual ao VSAM do enunciado):              *
      * CODCLI(6) + NOME(30) + TELEFONE(15) + CIDADE(20) = 71         *
      *--------------------------------------------------------------*
       01  WS-REG-CLIENTE.
           05  WS-CODCLI            PIC 9(6).
           05  WS-NOME              PIC X(30).
           05  WS-TELEFONE          PIC X(15).
           05  WS-CIDADE            PIC X(20).

       PROCEDURE DIVISION.
       0000-PRINCIPAL.
           OPEN OUTPUT ARQ-CLIENTES

           IF WS-FILE-STATUS NOT = "00"
              DISPLAY "Erro ao criar arquivo. STATUS: " WS-FILE-STATUS
              STOP RUN
           END-IF

           PERFORM 10 TIMES
               PERFORM 1000-MONTAR-E-GRAVAR
           END-PERFORM

           CLOSE ARQ-CLIENTES

           DISPLAY " "
           DISPLAY "Arquivo data/clientes.dat criado com sucesso."
           DISPLAY "Total de registros gravados: " WS-TOTAL-CARGA
           STOP RUN.

       1000-MONTAR-E-GRAVAR.
           ADD 1 TO WS-TOTAL-CARGA

           EVALUATE WS-TOTAL-CARGA
               WHEN 1
                   MOVE 000001                 TO WS-CODCLI
                   MOVE "JOAO DA SILVA"         TO WS-NOME
                   MOVE "(35)99100-1111"        TO WS-TELEFONE
                   MOVE "POUSO ALEGRE"          TO WS-CIDADE
               WHEN 2
                   MOVE 000002                 TO WS-CODCLI
                   MOVE "MARIA OLIVEIRA SANTOS" TO WS-NOME
                   MOVE "(35)99200-2222"        TO WS-TELEFONE
                   MOVE "VARGINHA"              TO WS-CIDADE
               WHEN 3
                   MOVE 000003                 TO WS-CODCLI
                   MOVE "CARLOS EDUARDO LIMA"   TO WS-NOME
                   MOVE "(31)98300-3333"        TO WS-TELEFONE
                   MOVE "BELO HORIZONTE"        TO WS-CIDADE
               WHEN 4
                   MOVE 000004                 TO WS-CODCLI
                   MOVE "ANA PAULA FERREIRA"    TO WS-NOME
                   MOVE "(11)97400-4444"        TO WS-TELEFONE
                   MOVE "SAO PAULO"             TO WS-CIDADE
               WHEN 5
                   MOVE 000005                 TO WS-CODCLI
                   MOVE "PEDRO HENRIQUE COSTA"  TO WS-NOME
                   MOVE "(21)96500-5555"        TO WS-TELEFONE
                   MOVE "RIO DE JANEIRO"        TO WS-CIDADE
               WHEN 6
                   MOVE 000006                 TO WS-CODCLI
                   MOVE "LUCIA MENDES ROCHA"    TO WS-NOME
                   MOVE "(35)95600-6666"        TO WS-TELEFONE
                   MOVE "ITAJUBA"               TO WS-CIDADE
               WHEN 7
                   MOVE 000007                 TO WS-CODCLI
                   MOVE "ROBERTO ALVES NETO"    TO WS-NOME
                   MOVE "(37)94700-7777"        TO WS-TELEFONE
                   MOVE "DIVINOPOLIS"           TO WS-CIDADE
               WHEN 8
                   MOVE 000008                 TO WS-CODCLI
                   MOVE "FERNANDA XAVIER PINTO" TO WS-NOME
                   MOVE "(32)93800-8888"        TO WS-TELEFONE
                   MOVE "JUIZ DE FORA"          TO WS-CIDADE
               WHEN 9
                   MOVE 000009                 TO WS-CODCLI
                   MOVE "MARCOS ANTONIO SOUZA"  TO WS-NOME
                   MOVE "(35)92900-9999"        TO WS-TELEFONE
                   MOVE "ALFENAS"               TO WS-CIDADE
               WHEN 10
                   MOVE 000010                 TO WS-CODCLI
                   MOVE "PATRICIA GOMES VIEIRA" TO WS-NOME
                   MOVE "(35)91000-0000"        TO WS-TELEFONE
                   MOVE "TRES CORACOES"         TO WS-CIDADE
           END-EVALUATE

           MOVE WS-REG-CLIENTE TO REG-LINHA
           WRITE REG-LINHA.
