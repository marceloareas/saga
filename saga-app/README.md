# Gerdisc 

## Executar Docker Compose
1. No terminal ou prompt de comando, mude o diretório de trabalho para o diretório onde está a pasta `\saga-app`.
2. Verifique se o Docker App está rodando.
3. Verifique a existência do arquivo `docker-compose.yaml`
4. Execute o comando `docker-compose up -d`
5. Ou execute o comando `docker compose --env-file .env up` para start os containers com as variáveis de .env

Após a execução do Docker Compose, é possível verificar as telas da aplicação consultando a URL `http://localhost:3000/saga/`. Também é possível consultar o [Swagger com o Catálogo de APIs](https://github.com/ribeiroisaac/saga/wiki/Gerdisc-%E2%80%90-Swagger-API) e informações dos [Bancos de Dados migrados](https://github.com/ribeiroisaac/saga/wiki/Gerdisc-%E2%80%90-Conex%C3%A3o-com-o-Banco). Para mais informações, consulte o manual do desenvolvedor.

não funciona consultar a url após rodar o rocket no github workspace, eu (pedrojaoulack) tentei e não funcionou :( 

## Shutdown de containers
Para interromper o container criado no Docker Compose, siga as etapas seguintes:
1. No terminal ou prompt de comando, mude o diretório de trabalho para o diretório onde está a pasta `\saga\gerdisc`.
2. Execute o comando `docker compose down`
