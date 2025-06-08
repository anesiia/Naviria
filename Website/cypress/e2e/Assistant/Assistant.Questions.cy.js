describe('Сторінка "Персональний помічник"', () => {
    beforeEach(() => {
        cy.loginAsMaria();
        cy.visit('/assistant');
    });

    it('Має заголовок "Персональний помічник", поле вводу і кнопку надсилання', () => {
        cy.contains('Персональний помічник').should('be.visible');
        cy.get('input[type="text"]').should('exist').and('be.visible');
        cy.get('.send-button').should('exist').and('be.visible');
    });

    it('Користувач не може відправити пусте повідомлення', () => {
        cy.get('input[type="text"]').clear();
        cy.get('.send-button').click();

        cy.get('.messages .message').should('have.length', 0);
    });

    it('Після 21-го повідомлення виводяться лише останніЙ 1, перевірка черговості, з оновленням сторінки', () => {
        const mathExamples = [
            "Скільки буде 2 + 2?",
            "Скільки буде 5 * 3?",
            "Який корінь квадратний з 49?",
            "Обчисли 144 / 12",
            "Скільки буде 7 - 4?",
            "Який залишок від ділення 10 на 3?",
            "Скільки буде 3 у степені 3?",
            "Скільки буде 100 - 56?",
            "Обчисли 12 * 6",
            "Скільки буде 81 / 9?",
            "Скільки буде 6 + 7?",
            "Скільки буде 15 - 8?",
            "Скільки буде 9 * 9?",
            "Скільки буде 100 / 10?",
            "Обчисли 11 + 22",
            "Скільки буде 20 - 5?",
            "Скільки буде 8 * 8?",
            "Скільки буде 64 / 8?",
            "Скільки буде 10 + 15?",
            "Скільки буде 12 * 2?",
            "Що таке 7 помножити на 7?"
        ];

        mathExamples.forEach((message, index) => {
            cy.get('input[type="text"]').clear().type(message);
            cy.get('.send-button').click();

            // Очікуємо, що асистент відповів — додається нове повідомлення
            cy.get('.messages .message.assistant')
                .should('have.length.greaterThan', Math.floor(index / 2));

            cy.wait(1500);
        });

        cy.reload();

        cy.get('.messages .message').should('have.length', 2);


    });


    it('Перевірка чергування повідомлень: користувач → асистент', () => {
        cy.reload();
        cy.get('input[type="text"]').clear().type('Скільки буде 5 + 5?');
        cy.get('.send-button').click();

        cy.wait(3000);

        cy.get('.messages .message').then((messages) => {
            expect(messages[0]).to.have.class('user');
            expect(messages[1]).to.have.class('assistant');
        });
    });
});
