# Use an official Node.js 20.x runtime as the base image
FROM node:20

# Set the working directory in the container
WORKDIR /usr/src/app

# Copy the package.json and package-lock.json files to the container
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the rest of the application's code
COPY . .

# Specify the command to run when the container starts
CMD ["npm", "start"]
