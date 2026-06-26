# ---------------------------------------------------------------------
# Makefile - Projeto 7 COBOL - Sistema de Consulta e Atualizacao de
#            Clientes (transacao CLIE / programa CLIPGM)
#
# Uso:
#   make            -> compila, gera a massa de dados e roda os testes
#   make build      -> so compila CARGA e CLIPGM
#   make data       -> compila (se preciso) e gera data/clientes.dat
#   make test       -> compila + gera dados + roda testes.sh
#   make run        -> compila + gera dados + executa o CLIPGM interativo
#   make clean      -> remove binarios e o arquivo de dados gerado
# ---------------------------------------------------------------------

COBC      := cobc
CFLAGS    := -x

SRC_DIR   := src
DATA_DIR  := data

CARGA_SRC  := $(SRC_DIR)/CARGA.cbl
CLIPGM_SRC := $(SRC_DIR)/CLIPGM.cbl

CARGA_BIN  := CARGA
CLIPGM_BIN := CLIPGM

DATA_FILE  := $(DATA_DIR)/clientes.dat

.PHONY: all build data test run clean

all: test

# ---------------------------------------------------------------------
# Compilacao - o cobc gera o binario no diretorio atual (raiz),
# mesmo apontando para fontes dentro de src/
# ---------------------------------------------------------------------
build: $(CARGA_BIN) $(CLIPGM_BIN)

$(CARGA_BIN): $(CARGA_SRC)
	$(COBC) $(CFLAGS) $(CARGA_SRC)

$(CLIPGM_BIN): $(CLIPGM_SRC)
	$(COBC) $(CFLAGS) $(CLIPGM_SRC)

# ---------------------------------------------------------------------
# Massa de dados - sempre regera o arquivo a partir do zero, para
# garantir que os testes partam de um estado conhecido
# ---------------------------------------------------------------------
data: build
	rm -f $(DATA_FILE)
	./$(CARGA_BIN)

# ---------------------------------------------------------------------
# Testes - usa o testes.sh, que ja faz seu proprio reset de dados
# ---------------------------------------------------------------------
test: build
	bash testes.sh

# ---------------------------------------------------------------------
# Execucao interativa do programa principal
# ---------------------------------------------------------------------
run: data
	./$(CLIPGM_BIN)

# ---------------------------------------------------------------------
# Limpeza
# ---------------------------------------------------------------------
clean:
	rm -f $(CARGA_BIN) $(CLIPGM_BIN)
	rm -f $(DATA_FILE)