describe("Помічник створює задачі за запитом", () => {
    beforeEach(() => {
        cy.loginAsMaria();
        cy.visit("/assistant");
    });

    it("обранно запит на створення, але не можливо відправити пусте повідомлення", () => {
        // Включити чекбокс створення задачі
        cy.get("#create-task-toggle").check().should("be.checked");

        // Спробувати відправити пусте повідомлення
        cy.get('input[placeholder="Type message"]').clear();
        cy.get(".send-button").click();

        // Переконатись, що повідомлення не додалось (число повідомлень не змінилось)
        cy.get(".messages .message.user").then(($messages) => {
            const initialCount = $messages.length;
            // Натискаємо кнопку ще раз (порожній інпут)
            cy.get(".send-button").click();
            cy.get(".messages .message.user").should("have.length", initialCount);
        });
    });

    it('повинно створити завдання, якщо обрано "запит на створення задачі"', () => {
        // Включити чекбокс створення задачі
        cy.get("#create-task-toggle").check();

        // Ввести повідомлення, яке створить задачу
        const taskTitle = "Хочу вивчити данську мову з 0";
        const keywords = ["вивчити", "данську"];

        cy.get('input[placeholder="Type message"]')
            .clear()
            .type(taskTitle);

        cy.get(".send-button").click();

        // Перевірити, що з'явилась відповідь від асистента з текстом, що підтверджує створення задачі
        cy.get(".messages .message.assistant", { timeout: 10000 })
            .invoke('text')
            .should('match', /.* Задача .* створена .*/);

        cy.wait(2000);

        cy.visit("/tasks");

        cy.get(".folders").should("exist");

        // Знайти папку "Generated tasks" і відкрити її
        cy.contains(".folder", "Generated tasks").click();

        cy.wait(1000);

        cy.get(".tasks .task-label").should(($els) => {
            const texts = $els.map((i, el) => Cypress.$(el).text().toLowerCase()).get();

            // Перевіряємо, що є хоча б один елемент, в якому всі ключові слова присутні
            const hasMatch = texts.some(text =>
                keywords.every(word => text.includes(word))
            );

            expect(hasMatch).to.be.true;
        });
    });
});
