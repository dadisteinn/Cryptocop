const creditcard = require("creditcard");
const amqp = require("amqplib/callback_api");

const messageBrokerInfo = {
  exchanges: {
    order: "order-exchange",
  },
  queues: {
    paymentQueue: "payment-queue",
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
        const cardNum = dataJson["CreditCard"];
        const cardIsValid = creditcard.validate(cardNum);

        console.log(
          "Card number " +
            cardNum +
            (cardIsValid ? " is valid" : " is NOT valid")
        );
      } catch (err) {
        console.log(err);
      }
    },
    { noAck: true }
  );
})().catch((e) => console.error(e));
