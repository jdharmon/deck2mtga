# Deck2MTGA

Web application/API to convert standard text decklists into Magic: The Gathering Arena format.

### Usage:

```
docker run -p 8080:80 -e LEGAL_SETS=AKH;HOU;XLN;RIX jdharmon/deck2mtga
```

The environment variable ```LEGAL_SETS``` defines which sets to search.

Uses the [Scryfall API](https://scryfall.com/docs/api) and [C# Scryfall Client](https://github.com/jdharmon/scryfallapi-csharp).