Type Extension Guidelines:
* All methods should only hit the database if the data has not been pre-populated
* If the database is hit, update the entity object with the retrieved data to prevent requeries
* Ignore properties to prevent naming conflicts.  (E.G. If BillingContact is a guid field but you want it to return the actual LegalEntity)
* Ignore properties to hydrate properties if they have not already been hydrated.
* Do not ignore properties that have more convenient type extention properties.  (E.G. Don't hide IdNavigation because name and contact info can be gotten by type extensions)  This allows metadata like Created or Modified to be accessed.
* Only write methods for data you need or expect to need.
* Consider type extensions instead of additional root level query methods.  (E.G.  Instead of GetAccountsByAgent, consider a type extension on Agent that gets related accounts.)
