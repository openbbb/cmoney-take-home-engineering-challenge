# Take Home Engineering Challenge

## Main Problem
* 依照`證券代號` 搜尋最近n天的資料 

`[HttpGet] /api/v1/StockExchange?stockid=1101`

```json
{
    "stockExchanges": [
        {
            "stockid": 1101,
            "createdAt": "2020-12-31T00:00:00",
            "name": "台泥",
            "yield": 6.94,
            "yieldyear": 108,
            "pe": 10.29,
            "pb": 1.32,
            "reportyear": "109/3"
        },
        {
            "stockid": 1101,
            "createdAt": "2021-01-04T00:00:00",
            "name": "台泥",
            "yield": 6.94,
            "yieldyear": 108,
            "pe": 10.29,
            "pb": 1.32,
            "reportyear": "109/3"
        },
        {
            "stockid": 1101,
            "createdAt": "2021-01-05T00:00:00",
            "name": "台泥",
            "yield": 6.96,
            "yieldyear": 108,
            "pe": 10.26,
            "pb": 1.32,
            "reportyear": "109/3"
        }
    ],
    "httpCode": "00",
    "httpMessage": null
}
```

* 指定`特定日期` 顯示當天`本益比`前n名

`[HttpGet] /api/v1/StockExchange?date=2020/12/31`
```json
{
    "stockExchanges": [
        {
            "stockid": 6698,
            "createdAt": "2020-12-31T00:00:00",
            "name": "旭暉應材",
            "yield": 3.62,
            "yieldyear": 108,
            "pe": 4425,
            "pb": 2.42,
            "reportyear": "109/3"
        },
        {
            "stockid": 2236,
            "createdAt": "2020-12-31T00:00:00",
            "name": "百達-KY",
            "yield": 2.13,
            "yieldyear": 108,
            "pe": 1975,
            "pb": 0.78,
            "reportyear": "109/3"
        }
    ],
    "httpCode": "00",
    "httpMessage": null
}
```

* 指定`日期範圍`、`證券代號` 顯示這段時間內`殖利率` 為嚴格遞增的最長天數並顯示開始、結束日期

`[HttpGet] /api/v1/StockExchange?startdate=2020/12/31&enddate=2021/01/06&stockid=1101`
```json
{
    "stockExchanges": [
        {
            "stockid": 1101,
            "createdAt": "2021-01-05T00:00:00",
            "name": "台泥",
            "yield": 6.96,
            "yieldyear": 108,
            "pe": 10.26,
            "pb": 1.32,
            "reportyear": "109/3"
        },
        {
            "stockid": 1101,
            "createdAt": "2020-12-31T00:00:00",
            "name": "台泥",
            "yield": 6.94,
            "yieldyear": 108,
            "pe": 10.29,
            "pb": 1.32,
            "reportyear": "109/3"
        },
        {
            "stockid": 1101,
            "createdAt": "2021-01-04T00:00:00",
            "name": "台泥",
            "yield": 6.94,
            "yieldyear": 108,
            "pe": 10.29,
            "pb": 1.32,
            "reportyear": "109/3"
        }
    ],
    "httpCode": "00",
    "httpMessage": null
}
```

## 資料庫寫入 個股日本益比、殖利率及股價淨值比
`[HttpPost] /api/v1/StockExchange`

Body 指定 `date` 日期

```json
{
  "date":"2021/01/07"
}
```

