[TOC]



# New Rates Calculation Algorithm Features

- [x] Whole Rate is loadable in memory as Single Object
- [x] Easy usage. In order to obtain amount, you just need to invoke on Rate an extension method: Calculate Amount to Pay.
- [x] x10-20 times faster than older one
- [x] Optimized for long range calculations. Periods like 1 month/year and so on.
- [x] For now beta support for Cross-Day Belts calculations
- [x] Supports Festivities and Special Days and obviously Normal Days
- [x] Built in Net5.0
- [x] Used Divide-Et-Impera Iterative Cumulative approach
- [x] Rich expandible logging in Trace mode
- [x] Standalone UnitTests for faster development and integrity maintainance



# How to test RATES

1. Edit App.Config of: Console_Server_RTS in order to point to correct DB
2. Start Console_Server_RTS
3. Wait until starts completely
4. Launch BloomRPC
5. Import: ...\ParkO_V3\ADVANCED_RTS\Shared_API_RTS\Generic_API.proto
6. Set correct destination Ip and Port
7. Call PullRate (it will autodetect caller ip, in case of same machine, ip will be 127.0.0.1)
8. Ensure to have device on MainDB in appropriate Park with that IP (preferebly some kind of Column)
9. Invoke PullRate
10. You can easily preview the result on sites like this: http://jsonviewer.stack.hu/
11. Once you are satisfied with result, under UnitTestLibRtDb\Data
12. Create *.json file and paste all content there
13. Now you can write new test cases by loading in memory that json, and you even no need to have access to the DB from now on, until you will need to download new rates
14. Rates are easily created from WebAdmin