FROM node as node
EXPOSE 80

WORKDIR /app
COPY ["package*.json", "."]
RUN npm i -g @angular/cli@17
RUN npm i
COPY . .
RUN npm run build --configuration=prod

FROM nginx as final
COPY --from=node /app/dist/prod /usr/share/nginx/html
