-- デザイアデビル ベッシー

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnACT)
	ability1.SetLocation(l.RC)
	ability1.SetCondition("Condition")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Condition()
	return obj.Exists({q.Location, l.PlayerUnits, q.Count, 5})
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function Activation()
	local power
	if obj.Exists({q.Location, l.Damage, q.Count, 6}) then
		power = 15000
	else
		power = 5000
	end
	obj.Select({q.Location, l.PlayerVC, q.Count, 1})
	obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, power, p.UntilEndOfTurn)
end