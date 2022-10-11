module test 'br:steffesmoduler.azurecr.io/br-steffes-moduler-api:latest' = {
  image: 'br:steffesmoduler.azurecr.io/br-steffes-moduler-api:latest'
  container_name: 'br-steffes-moduler-api'
  environment: {
    PORT: '8080'
  }
}
