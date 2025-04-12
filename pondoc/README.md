## Pondoc
O Pondoc é uma ferramenta para produção de relatório em excel para analise de pontuação de docentes a partir de produções bibliográficas do Programa de Pós-graduação em Ciência da Computação (PPCIC) do CEFET-RJ. Ele leva em consideração a avaliação Qualis das conferências e das revistas de publicação.

## Executar Docker Compose
1. No terminal ou prompt de comando, mude o diretório de trabalho para o diretório onde está a pasta `\saga\pondoc`.
2. Verifique a existência do arquivo `docker-compose.yaml`
3. Execute o comando `docker-compose up -d`

Após a execução do Docker Compose, é possível verificar as telas da aplicação consultando a URL `http://localhost:3000/`. 

## Shutdown de containers
Para interromper o container criado no Docker Compose, siga as etapas seguintes:
1. No terminal ou prompt de comando, mude o diretório de trabalho para o diretório onde está a pasta `\saga\gerdisc`.
2. Execute o comando `docker compose down`
