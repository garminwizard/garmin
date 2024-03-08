VO2 max multisport: https://support.garmin.com/en-IN/?faq=DrBIn5T0TX3PH4LBYokzz9

Specifications for one product:
https://www.garmin.com/en-US/compare/?compareProduct=873008

List of all products:
https://www.garmin.com/c/api/getProducts?categoryKey=10002&locale=en-US&storeCode=US

Product prices:
https://www.garmin.com/c/api/getProductsPrice?productIds=621922&productIds=628939&productIds=641121&productIds=646690&productIds=698632&productIds=713363&productIds=731641&productIds=741137&productIds=777655&productIds=777730&productIds=780139&productIds=780154&productIds=780165&productIds=780196&productIds=886689&productIds=886725&productIds=886785&countryCode=NO&storeCode=NO&locale=nb-NO&categoryKey=10002&appName=www-category-pages&cg=none

select displayName, specKey, specDisplayValue from products where specKey like "productSpecV02Max";

select displayName, specKey, specDisplayValue from products where specKey like "productSpecV02Max" or specKey like "%productSpecFeaturePowerMeterComp%";

SELECT displayName
FROM Products
WHERE specKey = 'productSpecV02Max' AND specDisplayValue = 'Yes'
AND displayName IN (
    SELECT displayName
    FROM Products
    WHERE specKey = 'productSpecFeaturePowerMeterComp' AND specDisplayValue = 'Yes'
)
ORDER BY displayName;
