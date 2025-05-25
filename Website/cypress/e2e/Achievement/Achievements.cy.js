describe('Сторінка "Досягнення"', () => {
    it('відображає досягнення для Алекса (авторизований користувач із досягненнями)', () => {
        cy.loginAsAlex();
        cy.visit('/achievements');

        cy.contains('Твої досягнення').should('be.visible');
        cy.get('.achievement').should('have.length.at.least', 1);
        cy.get('.gift-btn').first().should('contain', 'Подарунок');
    });

    it('не відображає досягнень для Марії (авторизований користувач без досягнень)', () => {
        cy.loginAsMaria();
        cy.visit('/achievements');

        cy.contains('Твої досягнення').should('be.visible');

        // Перевірка, що немає жодного елемента з класом 'achievement'
        cy.get('.achievement').should('have.length', 0);

        // Опційно, якщо на сторінці є повідомлення про відсутність досягнень, перевір його
        cy.contains('Немає досягнень').should('be.visible');
    });
});

