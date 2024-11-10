curl -X POST http://localhost:8080/game \
-H "x-api-key: 05ae5782-1936-4c6a-870b-f3d64089dcf5" \
-H "Content-Type: application/json" \
-d '{
           "MapName": "Gothenburg",
           "Proposals": [
             {
               "CustomerName": "Customer1",
               "MonthsToPayBackLoan": 12,
               "YearlyInterestRate": 0.1
             },
             {
               "CustomerName": "Customer2",
               "MonthsToPayBackLoan": 12,
               "YearlyInterestRate": 0.1
             }
           ],
           "Iterations": [
             {
               "Customer1": {
                 "Type": "Award",
                 "Award": "IkeaFoodCoupon"
               },
               "Customer2": {
                 "Type": "Skip",
                 "Award": "None"
               }
             }
           ]
         }'
