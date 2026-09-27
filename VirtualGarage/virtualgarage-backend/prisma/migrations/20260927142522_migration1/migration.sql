-- CreateTable
CREATE TABLE `users` (
    `id` CHAR(36) NOT NULL,
    `firstName` VARCHAR(50) NOT NULL,
    `lastName` VARCHAR(50) NOT NULL,
    `email` VARCHAR(200) NOT NULL,
    `birthDay` DATETIME(3) NOT NULL,
    `passwordHash` VARCHAR(255) NOT NULL,
    `userRole` VARCHAR(20) NOT NULL DEFAULT 'user',

    UNIQUE INDEX `users_email_key`(`email`),
    PRIMARY KEY (`id`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- CreateTable
CREATE TABLE `vehicles` (
    `id` CHAR(36) NOT NULL,
    `userId` CHAR(36) NOT NULL,
    `brand` VARCHAR(50) NOT NULL,
    `model` VARCHAR(50) NOT NULL,
    `manufactureDate` DATETIME(3) NOT NULL,

    INDEX `vehicles_userId_idx`(`userId`),
    PRIMARY KEY (`id`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- CreateTable
CREATE TABLE `donations` (
    `id` CHAR(36) NOT NULL,
    `userId` CHAR(36) NOT NULL,
    `amount` INTEGER NOT NULL,
    `currency` VARCHAR(10) NOT NULL,
    `email` VARCHAR(100) NOT NULL,
    `paymentIntentId` VARCHAR(255) NOT NULL,
    `status` VARCHAR(50) NOT NULL,
    `createdAt` DATETIME(3) NOT NULL,
    `notes` VARCHAR(191) NULL,

    INDEX `donations_userId_idx`(`userId`),
    PRIMARY KEY (`id`)
) DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

-- AddForeignKey
ALTER TABLE `vehicles` ADD CONSTRAINT `vehicles_userId_fkey` FOREIGN KEY (`userId`) REFERENCES `users`(`id`) ON DELETE CASCADE ON UPDATE CASCADE;

-- AddForeignKey
ALTER TABLE `donations` ADD CONSTRAINT `donations_userId_fkey` FOREIGN KEY (`userId`) REFERENCES `users`(`id`) ON DELETE CASCADE ON UPDATE CASCADE;
