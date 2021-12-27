-- 聖裁の刻, 来たれり

function RegisterAbilities()
	-- order
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnOrder)
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Cost(check)
	if check then return obj.CanCB(2) end
	obj.CounterBlast(2)
end

function Activation()
	obj.Draw(2)
	obj.Select({q.Location, l.PlayerUnits, q.Count, 1})
	obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 5000, p.UntilEndOfTurn)
end