# Deck2MTGA

Web application/API to convert standard text decklists into Magic: The Gathering Arena format.

### Usage:

```
docker run -p 8080:80 -e LEGAL_SETS=AKH;HOU;XLN;RIX jdharmon/deck2mtga
```

The environment variable ```LEGAL_SETS``` defines which sets to search.

Uses the [MTGDb](https://github.com/jdharmon/MTGDb) and data from [MTG JSON](https://mtgjson.com/).
