      *----------------------------------------------------------------*
      * CLIPGM_CICS.CBL                                                *
      * VERSAO "DE PAPEL" - FIEL AO CICS REAL                          *
      *                                                                *
      * ATENCAO: este programa NAO compila no GnuCOBOL nem em          *
      * nenhum compilador COBOL comum. Ele usa comandos EXEC CICS,     *
      * que so existem em um ambiente mainframe real com CICS          *
      * Transaction Server e um TRADUTOR CICS (pre-compilador) que     *
      * converte cada EXEC CICS em chamadas de API antes da            *
      * compilacao COBOL de fato. Sem esse tradutor, o codigo abaixo   *
      * nao tem como ser processado.                                  *
      *                                                                *
      * Apresentado como artefato de design / estudo, demonstrando    *
      * como o programa CLIPGM seria escrito de fato em producao,     *
      * para comparacao com a versao executavel (CLIPGM.cbl).         *
      *                                                                *
      * Transacao    : CLIE                                            *
      * Programa     : CLIPGM                                          *
      * Mapa / Mapset: CLIEMAP / CLIESET (ver CLIEMAP.bms e .cpy)      *
      * Arquivo VSAM : CLIENTES (KSDS, chave CODCLI)                   *
      *                                                                *
      * Campos especiais usados abaixo (resolvidos pelo TRADUTOR CICS, *
      * nao sao variaveis COBOL comuns):                              *
      *   EIBCALEN        - tamanho da COMMAREA recebida (0 = 1a vez) *
      *   EIBAID          - qual tecla foi pressionada (AID)          *
      *   DFHPF3/5/6      - valores especiais que representam PF3/5/6 *
      *   DFHRESP(NORMAL) - condicao de retorno normal de um comando  *
      *----------------------------------------------------------------*
       IDENTIFICATION DIVISION.
       PROGRAM-ID. CLIPGM.

       ENVIRONMENT DIVISION.

       DATA DIVISION.
       WORKING-STORAGE SECTION.

      *--------------------------------------------------------------*
      * Layout do registro VSAM CLIENTES (igual ao enunciado)         *
      *--------------------------------------------------------------*
       01  WS-REG-CLIENTE.
           05  WS-CODCLI            PIC 9(6).
           05  WS-NOME              PIC X(30).
           05  WS-TELEFONE          PIC X(15).
           05  WS-CIDADE            PIC X(20).

      *--------------------------------------------------------------*
      * Mensagens fixas do sistema                                    *
      *--------------------------------------------------------------*
       01  WS-MENSAGENS.
           05  MSG-ENCONTRADO       PIC X(30)
                                    VALUE "CLIENTE ENCONTRADO".
           05  MSG-NAO-ENCONTRADO   PIC X(30)
                                    VALUE "CLIENTE NAO ENCONTRADO".
           05  MSG-ALTERADO         PIC X(30)
                                    VALUE "ALTERACAO REALIZADA".
           05  MSG-CAMPO-OBRIG      PIC X(30)
                                    VALUE "CODIGO OBRIGATORIO".
           05  MSG-SEM-CONSULTA     PIC X(30)
                                    VALUE "CONSULTE ANTES DE SALVAR".
           05  MSG-ERRO-VSAM        PIC X(30)
                                    VALUE "ERRO AO ACESSAR ARQUIVO".

      *--------------------------------------------------------------*
      * Mapa simbolico - em ambiente real seria GERADO pelo tradutor *
      * BMS a partir de src/CLIEMAP.bms. Aqui foi escrito a mao       *
      * (ver src/CLIEMAP.cpy) so para fins didaticos.                 *
      *--------------------------------------------------------------*
           COPY CLIEMAP.

      *--------------------------------------------------------------*
      * Controle de comandos CICS                                     *
      *--------------------------------------------------------------*
       01  WS-RESP                  PIC S9(8) COMP.

      *--------------------------------------------------------------*
      * Copia de trabalho da COMMAREA - e o estado que "viaja" entre  *
      * as execucoes pseudo-conversacionais da transacao CLIE. Cada   *
      * Enter do usuario MATA o programa na memoria; sem a COMMAREA   *
      * o CICS nao teria como saber, no proximo Enter, qual cliente   *
      * estava sendo consultado.                                      *
      *--------------------------------------------------------------*
       01  WS-COMMAREA.
           05  CA-CODCLI             PIC 9(6).
           05  CA-NOME               PIC X(30).
           05  CA-TELEFONE           PIC X(15).
           05  CA-CIDADE             PIC X(20).
           05  CA-CLIENTE-CARREGADO  PIC X VALUE "N".

       LINKAGE SECTION.
       01  DFHCOMMAREA               PIC X(72).

       PROCEDURE DIVISION.

      *--------------------------------------------------------------*
      * EIBCALEN = 0 -> primeira vez que a transacao CLIE roda nesta  *
      * sessao (sem COMMAREA ainda). Equivalente, na versao           *
      * executavel, a primeira exibicao da tela em branco.            *
      *--------------------------------------------------------------*
       0000-PRINCIPAL.
           IF EIBCALEN = ZERO
               PERFORM 1000-PRIMEIRA-EXECUCAO
           ELSE
               MOVE DFHCOMMAREA TO WS-COMMAREA
               PERFORM 2000-PROCESSAR-TECLA
           END-IF.

      *--------------------------------------------------------------*
      * Primeira execucao: envia a tela em branco e devolve o        *
      * controle ao CICS, aguardando o usuario digitar e dar Enter.   *
      *--------------------------------------------------------------*
       1000-PRIMEIRA-EXECUCAO.
           MOVE LOW-VALUES TO CLIEMAPO

           EXEC CICS SEND MAP('CLIEMAP')
                           MAPSET('CLIESET')
                           ERASE
           END-EXEC

           EXEC CICS RETURN
                           TRANSID('CLIE')
                           COMMAREA(WS-COMMAREA)
                           LENGTH(LENGTH OF WS-COMMAREA)
           END-EXEC.

      *--------------------------------------------------------------*
      * Demais execucoes: le o que o usuario digitou + qual tecla    *
      * PF foi pressionada, processa, e devolve a tela atualizada.   *
      *--------------------------------------------------------------*
       2000-PROCESSAR-TECLA.
           EXEC CICS RECEIVE MAP('CLIEMAP')
                              MAPSET('CLIESET')
                              RESP(WS-RESP)
           END-EXEC

           EVALUATE EIBAID
               WHEN DFHPF5
                   PERFORM 3000-CONSULTAR
               WHEN DFHPF6
                   PERFORM 4000-SALVAR
               WHEN DFHPF3
                   PERFORM 5000-SAIR
               WHEN OTHER
                   MOVE "OPCAO INVALIDA" TO MENSAGEMO
           END-EVALUATE

           IF EIBAID NOT = DFHPF3
               EXEC CICS SEND MAP('CLIEMAP')
                               MAPSET('CLIESET')
                               DATAONLY
               END-EXEC

               EXEC CICS RETURN
                               TRANSID('CLIE')
                               COMMAREA(WS-COMMAREA)
                               LENGTH(LENGTH OF WS-COMMAREA)
               END-EXEC
           END-IF.

      *--------------------------------------------------------------*
      * PF5 - Consultar cliente pelo codigo digitado.                 *
      *--------------------------------------------------------------*
       3000-CONSULTAR.
           IF CODCLII = ZERO
               MOVE MSG-CAMPO-OBRIG TO MENSAGEMO
           ELSE
               MOVE CODCLII TO WS-CODCLI

               EXEC CICS READ FILE('CLIENTES')
                               INTO(WS-REG-CLIENTE)
                               RIDFLD(WS-CODCLI)
                               RESP(WS-RESP)
               END-EXEC

               IF WS-RESP = DFHRESP(NORMAL)
                   MOVE WS-NOME     TO NOMEO
                   MOVE WS-TELEFONE TO TELEFONEO
                   MOVE WS-CIDADE   TO CIDADEO
                   MOVE MSG-ENCONTRADO TO MENSAGEMO

                   MOVE WS-CODCLI    TO CA-CODCLI
                   MOVE WS-NOME      TO CA-NOME
                   MOVE WS-TELEFONE  TO CA-TELEFONE
                   MOVE WS-CIDADE    TO CA-CIDADE
                   MOVE "S"          TO CA-CLIENTE-CARREGADO
               ELSE
                   MOVE MSG-NAO-ENCONTRADO TO MENSAGEMO
                   MOVE "N" TO CA-CLIENTE-CARREGADO
               END-IF
           END-IF.

      *--------------------------------------------------------------*
      * PF6 - Salvar alteracoes de TELEFONE e CIDADE.                 *
      * Usa READ ... UPDATE para reservar o registro antes do         *
      * REWRITE (evita que outro terminal altere o mesmo cliente      *
      * entre a leitura e a gravacao).                                 *
      *--------------------------------------------------------------*
       4000-SALVAR.
           IF CA-CLIENTE-CARREGADO NOT = "S"
               MOVE MSG-SEM-CONSULTA TO MENSAGEMO
           ELSE
               MOVE CA-CODCLI TO WS-CODCLI

               EXEC CICS READ FILE('CLIENTES')
                               INTO(WS-REG-CLIENTE)
                               RIDFLD(WS-CODCLI)
                               UPDATE
                               RESP(WS-RESP)
               END-EXEC

               IF WS-RESP = DFHRESP(NORMAL)
                   MOVE TELEFONEI TO WS-TELEFONE
                   MOVE CIDADEI   TO WS-CIDADE

                   EXEC CICS REWRITE FILE('CLIENTES')
                                      FROM(WS-REG-CLIENTE)
                                      RESP(WS-RESP)
                   END-EXEC

                   IF WS-RESP = DFHRESP(NORMAL)
                       MOVE MSG-ALTERADO TO MENSAGEMO
                       MOVE TELEFONEI TO CA-TELEFONE
                       MOVE CIDADEI   TO CA-CIDADE
                   ELSE
                       MOVE MSG-ERRO-VSAM TO MENSAGEMO
                   END-IF
               ELSE
                   MOVE MSG-NAO-ENCONTRADO TO MENSAGEMO
               END-IF
           END-IF.

      *--------------------------------------------------------------*
      * PF3 - Encerrar a transacao.                                   *
      *                                                                *
      * Demonstra o uso de XCTL (um dos conceitos pedidos no          *
      * projeto): em vez de simplesmente devolver o controle ao       *
      * CICS com RETURN, transfere o controle para um programa de     *
      * menu (MENUPGM), sem retornar para quem chamou o CLIPGM.       *
      * E uma alternativa tipica em sistemas online com varias        *
      * transacoes encadeadas (ex: CLIE chamado a partir de um menu). *
      *                                                                *
      * Caso nao houvesse um programa de menu para onde voltar, a     *
      * alternativa mais simples seria apenas:                        *
      *     EXEC CICS RETURN END-EXEC.                                *
      *--------------------------------------------------------------*
       5000-SAIR.
           EXEC CICS XCTL PROGRAM('MENUPGM')
           END-EXEC.
