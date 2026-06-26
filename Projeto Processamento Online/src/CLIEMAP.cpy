      *----------------------------------------------------------------*
      * CLIEMAP.CPY                                                    *
      *                                                                *
      * Mapa simbolico do mapa CLIEMAP / mapset CLIESET.               *
      *                                                                *
      * Em um ambiente CICS real, este copybook NAO e escrito a mao:  *
      * ele e GERADO AUTOMATICAMENTE pelo tradutor BMS a partir do     *
      * fonte fisico (src/CLIEMAP.bms), durante a compilacao do        *
      * mapset. Como nao ha tradutor CICS/BMS disponivel neste         *
      * ambiente, este arquivo foi escrito manualmente, reproduzindo  *
      * a estrutura que o tradutor geraria, para fins de estudo e     *
      * para permitir a leitura do programa CLIPGM_CICS.cbl.          *
      *                                                                *
      * Padrao DFHMDF para cada campo definido no mapa fisico:        *
      *   xxxxL  - tamanho dos dados digitados pelo usuario (input)   *
      *   xxxxF  - flag de atributo (uso interno do BMS)               *
      *   xxxxA  - atributo do campo (protecao, cor, etc.)             *
      *   xxxxI  - dado de ENTRADA (o que o terminal recebeu)          *
      *   xxxxO  - dado de SAIDA (o que sera enviado para a tela)      *
      *                                                                *
      * Observacao: a estrutura abaixo segue o padrao didatico mais    *
      * comum (sem otimizacoes de byte exatas que o tradutor real      *
      * geraria) - suficiente para demonstrar RECEIVE MAP / SEND MAP.  *
      *----------------------------------------------------------------*
       01  CLIEMAPI.
           02  FILLER                   PIC X(12).
      *    --- CODCLI (entrada: codigo do cliente) ---
           02  CODCLIL                  PIC S9(4) COMP.
           02  CODCLIF                  PIC X.
           02  FILLER REDEFINES CODCLIF.
               03  CODCLIA              PIC X.
           02  CODCLII                  PIC 9(6).
      *    --- NOME (somente exibido apos consulta) ---
           02  NOMEL                    PIC S9(4) COMP.
           02  NOMEF                    PIC X.
           02  FILLER REDEFINES NOMEF.
               03  NOMEA                PIC X.
           02  NOMEI                    PIC X(30).
      *    --- TELEFONE (entrada: editavel no PF6) ---
           02  TELEFONEL                PIC S9(4) COMP.
           02  TELEFONEF                PIC X.
           02  FILLER REDEFINES TELEFONEF.
               03  TELEFONEA            PIC X.
           02  TELEFONEI                PIC X(15).
      *    --- CIDADE (entrada: editavel no PF6) ---
           02  CIDADEL                  PIC S9(4) COMP.
           02  CIDADEF                  PIC X.
           02  FILLER REDEFINES CIDADEF.
               03  CIDADEA              PIC X.
           02  CIDADEI                  PIC X(20).
      *    --- MENSAGEM (campo PROT - so existe para alinhar com o  ---
      *    --- mapa fisico; o terminal nunca envia dados deste campo --
           02  MENSAGEML                PIC S9(4) COMP.
           02  MENSAGEMF                PIC X.
           02  FILLER REDEFINES MENSAGEMF.
               03  MENSAGEMA            PIC X.
           02  MENSAGEMI                PIC X(30).

       01  CLIEMAPO REDEFINES CLIEMAPI.
           02  FILLER                   PIC X(12).
           02  FILLER                   PIC X(3).
           02  CODCLIO                  PIC 9(6).
           02  FILLER                   PIC X(3).
           02  NOMEO                    PIC X(30).
           02  FILLER                   PIC X(3).
           02  TELEFONEO                PIC X(15).
           02  FILLER                   PIC X(3).
           02  CIDADEO                  PIC X(20).
           02  FILLER                   PIC X(3).
           02  MENSAGEMO                PIC X(30).
