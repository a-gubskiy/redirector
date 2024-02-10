# Redirector

## Docker container

### Build
```bash
 docker build -t redirector:local .   
```


### Run
```bash
docker run -p 8003:8003 redirector:local
```


## Example config

```json
{
  "Redirects": [
    {
      "Source": "gymnasium.kiev.ua",
      "Destination": "https://andrew.gubskiy.com/blog/item/gymnasium-kiev-ua/"
    },
    {
      "Source": "agi.net.ua",
      "Destination": "https://andrew.gubskiy.com/agi"
    },
    {
      "Source": "torf.bar",
      "Destination": "https://torf.tv/bar"
    }
  ]
}
```