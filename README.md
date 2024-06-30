# Garmin products and specifications

The code in this repository compiles to a console app which can be called with the following arguments:

- get-product-list
- get-products-html-files
- extract-products-to-json-from-html-files
- get-prices
- create-table
- populate-database-from-json-files
- update-prices
- generate-html

When running the first time, the commands should be executed in the order above and will do the following:

- download a list of Garmin products and store in a json file.
- download the html files for each product in the json file.
- extract the specifications for each product from the html files and stores them in json files.
- download the prices for the products and store them in json files.
- create a sqlite3 database with a products table.
- populate the database with the products specifications.
- update the product prices in the database.
- generate a simple html file with an overview of all the different specifications.

# Build

`dotnet build`

# Run

`bin/Debug/net8.0/garmin command`

# Garmin wizard

- Download the [Garmin wizard](https://github.com/garminwizard/garminwizard-homepage)
- Copy the products.db to that projects `static/garmin` directory.
- Adapt the wizard to your needs. 