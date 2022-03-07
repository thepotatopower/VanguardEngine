-- ナチュラルチャープ メルリア

function RegisterAbilities()
	-- ACT
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnACT)
	ability1.SetLocation(l.RC)
	ability1.SetCondition("Condition")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Condition()
	return obj.Exists({q.Location, l.PlayerUnits, q.NameContains, obj.LoadName("Earnescorrect"), q.Other, o.DifferentNames, q.Count, 5})
end

function Cost(check)
	if check then return obj.CanRest({q.Other, o.This}) end
	obj.Rest({q.Other, o.This})
end

function CanFullyResolve()
	return obj.Exists({q.Location, l.Drop, q.Other, o.Order})
end

function Activation()
	obj.ChooseAddToHand({q.Location, l.Drop, q.Other, o.Order, q.Count, 1})
	obj.SendToBottom({q.Other, o.This})
end