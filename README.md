# Trellog

Trellog is a github action that lets you create a changelog from trello, from lists and outputs the changes to another
list.

## Config

Below is an example of a config to be able to run this action.

```yaml
- name: trellog
  id: trellog
  uses: sabuto/trellog-action@master
  with:
    apiKey: your-api-key-here
    apiToken: your-api-token-here
    inputList: 'list-id-here'
    outputList: 'list-id-here'
    version: '1.0.2'
    versionType: 'Major'
    cleanInputs: 'true'
```

The available inputs are as follows

```yaml
    apiKey:
      description: 'The trello Api key needed to interact with trello'
      required: true
    apiToken:
      description: 'The trello Api token needed to interact with trello'
      required: true
    inputList:
      description: 'The list to get the data for the changelog'
      required: true
    outputList:
      description: 'The list to write the release data to.'
      required: false
    outputFile: (not implemented at the moment)
      description: 'The file to output the changelog to, default will be /github/workspace/changelog.md'
      required: false
      default: '/github/workspace/changelog.md'
    version:
      description: 'Allows you to manually specify a version, if left empty will bump +1 depending on releaseType'
      required: false
    versionType:
      description: 'The type of release you want to bump, can be Major, Minor or Patch'
      required: false
    cleanInputs:
      description: 'Clean the input list from trello, deleting all of the cards'
      required: false
      default: 'false'
```