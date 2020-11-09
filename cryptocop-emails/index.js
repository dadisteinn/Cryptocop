const amqp = require("amqplib/callback_api");
require("dotenv").config();

var api_key = process.env.API_KEY;
var domain = process.env.DOMAIN;
var mailgun = require("mailgun-js")({ apiKey: api_key, domain: domain });

const messageBrokerInfo = {
  exchanges: {
    order: "order-exchange",
  },
  queues: {
    paymentQueue: "email-queue",
  },
  routingKeys: {
    createOrder: "create-order",
  },
};

const createMessageBrokerConnection = () =>
  new Promise((resolve, reject) => {
    amqp.connect("amqp://localhost", (err, conn) => {
      if (err) {
        reject(err);
      }
      resolve(conn);
    });
  });

const createChannel = (connection) =>
  new Promise((resolve, reject) => {
    connection.createChannel((err, channel) => {
      if (err) {
        reject(err);
      }
      resolve(channel);
    });
  });

const configureMessageBroker = (channel) => {
  const { order } = messageBrokerInfo.exchanges;
  const { paymentQueue } = messageBrokerInfo.queues;
  const { createOrder } = messageBrokerInfo.routingKeys;

  channel.assertExchange(order, "direct", { durable: true });
  channel.assertQueue(paymentQueue, { durable: true });
  channel.bindQueue(paymentQueue, order, createOrder);
};

(async () => {
  const messageBrokerConnection = await createMessageBrokerConnection();
  const channel = await createChannel(messageBrokerConnection);

  configureMessageBroker(channel);

  const { paymentQueue } = messageBrokerInfo.queues;

  channel.consume(
    paymentQueue,
    (data) => {
      try {
        const dataJson = JSON.parse(data.content.toString());

        var mailText = "Name: " + dataJson["FullName"];
        mailText +=
          "\n" +
          "Address: " +
          dataJson["StreetName"] +
          " " +
          dataJson["HouseNumber"] +
          ", " +
          dataJson["ZipCode"] +
          ", " +
          dataJson["City"] +
          ", " +
          dataJson["Country"];

        mailText += "\n" + "Date of order: " + dataJson["OrderDate"];
        mailText += "\n" + "Total price: " + dataJson["TotalPrice"];
        mailText += "\n\n" + "Order items: ";

        dataJson["OrderItems"].forEach((item) => {
          mailText +=
            "\n\n" +
            "Product identifier: " +
            item["ProductIdentifier"] +
            "\n" +
            "Quantity: " +
            item["Quantity"] +
            "\n" +
            "Unit price: " +
            item["UnitPrice"] +
            "\n" +
            "Total price: " +
            item["TotalPrice"];
        });

        var emailData = {
          from: `Sender <${process.env.EMAIL}>`,
          to: `${process.env.EMAIL}`,
          subject: "Cryptocop-email",
          text: mailText,
        };

        mailgun.messages().send(emailData, function (error, body) {
          if (error) {
            console.log(error);
          }
          console.log(body);
        });
      } catch (err) {
        console.log(err);
      }
    },
    { noAck: true }
  );
})().catch((e) => console.error(e));
