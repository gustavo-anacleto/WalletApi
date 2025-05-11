CREATE SCHEMA IF NOT EXISTS keycloak;

CREATE TABLE "Users"
(
    "Id"    BIGSERIAL PRIMARY KEY,
    "Name"  TEXT NOT NULL,
    "Email" TEXT NOT NULL
);

CREATE UNIQUE INDEX "IX_Users_Email" ON "Users" ("Email");

CREATE TABLE "Transactions"
(
    "Id"         BIGSERIAL PRIMARY KEY,
    "SenderId"   BIGINT      NOT NULL,
    "ReceiverId" BIGINT      NOT NULL,
    "Amount"     NUMERIC     NOT NULL,
    "Date"       TIMESTAMPTZ NOT NULL,
    CONSTRAINT "FK_Transactions_Users_SenderId"
        FOREIGN KEY ("SenderId") REFERENCES "Users" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Transactions_Users_ReceiverId"
        FOREIGN KEY ("ReceiverId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Transactions_SenderId" ON "Transactions" ("SenderId");
CREATE INDEX "IX_Transactions_ReceiverId" ON "Transactions" ("ReceiverId");

CREATE TABLE "Wallets"
(
    "Id"      BIGSERIAL PRIMARY KEY,
    "Balance" NUMERIC NOT NULL,
    "UserId"  BIGINT  NOT NULL,
    CONSTRAINT "FK_Wallets_Users_UserId"
        FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE UNIQUE INDEX "IX_Wallets_UserId" ON "Wallets" ("UserId");


INSERT INTO "Users" ("Name", "Email")
values ('admin', 'admin@provider.com'),
       ('Alice Santos', 'alice@example.com'),
       ('Bruno Lima', 'bruno@example.com'),
       ('Carla Mendes', 'carla@example.com');


INSERT INTO "Wallets" ("Balance", "UserId")
VALUES (0.00, 1),    -- Admin
       (1500.00, 2), -- Alice
       (2300.50, 3), -- Bruno
       (320.75, 4); -- Carla
