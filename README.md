# PokeAPI

## Installation process for Docker

- Install docker or docker desktop
- If on windows, you may need to enable Hyper-V, on an admin powershell window:
```
  Enable-WindowsOptionalFeature -Online -FeatureName Microsoft-Hyper-V -All
``` 
- Download the image from docker hub

```
  docker pull giordanocampogiani/pokeapi:latest
``` 

- expose the host ports (standard 3000 and 3001)
- run the container, installation complete

## Available endpoints

### /api/pokemon/"pokemonName"

Retrieve basic information about your favourite pokemon such as:

- name
- description
- legendary status
- id
- habitat

### /api/pokemon/translated/"pokemonName"

All of the above, but using Funtranslation's API the following changes will be made to the description:

- If the Pokemon’s habitat is cave or it’s a legendary Pokemon then the Yoda translation will be applied.
- For all other Pokemon, the Shakespeare translation will be applied.
