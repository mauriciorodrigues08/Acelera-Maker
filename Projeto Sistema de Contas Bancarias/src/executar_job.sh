#!/bin/bash
# ==============================================================
# SCRIPT: executar_job.sh
# DESCRICAO: ORQUESTRA A EXECUCAO DO JOB BATCH COMPLETO:
#            CLILOAD -> TRXPROC -> RELATORIO
#
# CODIGOS DE RETORNO DOS PROGRAMAS COBOL:
#   RC 0 = SUCESSO TOTAL (SEM ERROS DE NEGOCIO)
#   RC 4 = ATENCAO (ERROS DE NEGOCIO DENTRO DO LIMITE, <= 20%)
#   RC 8 = ERRO GRAVE (ERROS DE NEGOCIO ACIMA DO LIMITE, > 20%)
# ==============================================================

set -u
cd ~ || exit 1

echo "=================================================="
echo " INICIANDO JOB BATCH - SISTEMA DE CONTAS BANCARIAS"
echo " DATA/HORA: $(date '+%d/%m/%Y %H:%M:%S')"
echo "=================================================="

RC_MAXIMO=0

# --------------------------------------------------------------
# STEP 0 - ORDENAÇÃO DA ENTRADA DE DADOS
# --------------------------------------------------------------
echo ""
echo "--- STEP 0: ORDENACAO DE DADOS ---"

# Ordenando CLIENTES.TXT 
sort CLIENTES.TXT -o CLIENTES.TXT
if [ $? -eq 0 ]; then
  echo ">>> CLIENTES.TXT ORDENADO COM SUCESSO"
else
  echo "*** ERRO AO ORDENAR CLIENTES.TXT ***"
  exit 1
fi

# Ordenando TRANSACOES.TXT
sort TRANSACOES.TXT -o TRANSACOES.TXT
if [ $? -eq 0 ]; then
  echo ">>> TRANSACOES.TXT ORDENADO COM SUCESSO"
else
  echo "*** ERRO AO ORDENAR TRANSACOES.TXT ***"
  exit 1
fi

# --------------------------------------------------------------
# STEP 1 - CLILOAD
# --------------------------------------------------------------
echo ""
echo "--- STEP 1: CLILOAD (CARGA DE CLIENTES) ---"
./CLILOAD
RC_CLILOAD=$?
echo ">>> CLILOAD FINALIZADO COM RC=${RC_CLILOAD}"

if [ ${RC_CLILOAD} -gt ${RC_MAXIMO} ]; then
    RC_MAXIMO=${RC_CLILOAD}
fi

if [ ${RC_CLILOAD} -ge 8 ]; then
    echo ""
    echo "*** JOB ABORTADO: CLILOAD RETORNOU RC=${RC_CLILOAD} (ERRO GRAVE) ***"
    echo "*** TRXPROC E RELATORIO NAO SERAO EXECUTADOS ***"
    exit ${RC_CLILOAD}
fi

# --------------------------------------------------------------
# STEP 2 - TRXPROC
# --------------------------------------------------------------
echo ""
echo "--- STEP 2: TRXPROC (PROCESSAMENTO DE TRANSACOES) ---"
./TRXPROC
RC_TRXPROC=$?
echo ">>> TRXPROC FINALIZADO COM RC=${RC_TRXPROC}"

if [ ${RC_TRXPROC} -gt ${RC_MAXIMO} ]; then
    RC_MAXIMO=${RC_TRXPROC}
fi

if [ ${RC_TRXPROC} -ge 8 ]; then
    echo ""
    echo "*** JOB ABORTADO: TRXPROC RETORNOU RC=${RC_TRXPROC} (ERRO GRAVE) ***"
    echo "*** RELATORIO NAO SERA EXECUTADO ***"
    exit ${RC_TRXPROC}
fi

# --------------------------------------------------------------
# STEP 3 - RELATORIO
# --------------------------------------------------------------
echo ""
echo "--- STEP 3: RELATORIO (RELATORIOS E ESTATISTICAS) ---"
./RELATORIO
RC_RELATORIO=$?
echo ">>> RELATORIO FINALIZADO COM RC=${RC_RELATORIO}"

if [ ${RC_RELATORIO} -gt ${RC_MAXIMO} ]; then
    RC_MAXIMO=${RC_RELATORIO}
fi

# --------------------------------------------------------------
# RESUMO FINAL DO JOB
# --------------------------------------------------------------
echo ""
echo "=================================================="
echo " RESUMO DA EXECUCAO DO JOB"
echo "=================================================="
echo " CLILOAD....: RC=${RC_CLILOAD}"
echo " TRXPROC....: RC=${RC_TRXPROC}"
echo " RELATORIO..: RC=${RC_RELATORIO}"
echo " RC MAXIMO DO JOB: ${RC_MAXIMO}"
echo "=================================================="

case ${RC_MAXIMO} in
    0)
        echo " STATUS GERAL: SUCESSO TOTAL"
        ;;
    4)
        echo " STATUS GERAL: SUCESSO COM ATENCAO (ERROS DENTRO DO LIMITE)"
        ;;
    *)
        echo " STATUS GERAL: ERRO GRAVE"
        ;;
esac

echo "=================================================="
echo " FIM DO JOB BATCH"
echo "=================================================="

exit ${RC_MAXIMO}
