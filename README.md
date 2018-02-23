# CSharp social app
Multilingual .NET authenticated app about article sharing.

## Disclaimer
I was not responsible for the RSS flux.

## Article flow
An article is created by an admin, but without actual content.
Then a writer creates the content of an article that must be validated in order
for it to be accessible publicly. Messages can be sent by admins to remind others
that an article need validation if need be.
Translations can also be submitted by translators.

The website displays the appropriate translation (locale language) if it exists,
else english by default. 

## Roles
4 different users:
- admin: creates users, can edit any data.
- writer: creates articles
- translator: translates article, validates other translator's articles
- visitor: not authenticated, can read validated articles

## Back office
Statistics about articles and users are accessible through the admin panel.

## RSS
A RSS flux of the new articles has been made:
- title of the article
- keywords about the article (publication date, validation date, author)
- article URL