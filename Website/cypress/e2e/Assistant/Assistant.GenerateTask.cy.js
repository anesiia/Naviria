describe("Помічник створює задачі за запитом", () => {
    beforeEach(() => {
        cy.loginAsMaria();
        cy.visit("/assistant");
    });

    it("обранно запит на створення, але не можливо відправити пусте повідомлення", () => {
        cy.get("#create-task-toggle").check().should("be.checked");

        cy.get('input[placeholder="Type message"]').clear();
        cy.get(".send-button").click();

        cy.get(".messages .message.user").then(($messages) => {
            const initialCount = $messages.length;
            // Натискаємо кнопку ще раз (порожній інпут)
            cy.get(".send-button").click();
            cy.get(".messages .message.user").should("have.length", initialCount);
        });
    });

    it('повинно створити завдання, якщо обрано "запит на створення задачі"', () => {
        cy.get("#create-task-toggle").check();

        // Ввести повідомлення, яке створить задачу
        const taskTitle = "Хочу вивчити данську мову з 0";
        const keywords = ["вивчити", "данську"];

        cy.get('input[placeholder="Type message"]')
            .clear()
            .type(taskTitle);

        cy.get(".send-button").click();

        cy.get(".messages .message.assistant", { timeout: 10000 })
            .invoke('text')
            .should('match', /.* Задача .* створена .*/);

        cy.wait(2000);

        cy.visit("/tasks");

        cy.get(".folders").should("exist");

        cy.contains(".folder", "Generated tasks").click();

        cy.wait(1000);

        cy.get(".tasks .task-label").should(($els) => {
            const texts = $els.map((i, el) => Cypress.$(el).text().toLowerCase()).get();

            const hasMatch = texts.some(text =>
                keywords.every(word => text.includes(word))
            );

            expect(hasMatch).to.be.true;
        });
    });
});
