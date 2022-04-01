-- 奇想機獣 バグモーター

function RegisterAbilities()
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
	return not obj.Exists({q.Location, l.PlayerPrisoners, q.Count, 2})
end

function Cost(check)
	if check then return obj.CanRest({q.Other, o.This}) end
	obj.Rest({q.Other, o.This})
end

function CanFullyResolve()
	return obj.HasPrison() and obj.Exists({q.Location, l.EnemyDrop})
end

function Activation()
	obj.ChooseImprison({q.Location, l.EnemyDrop, q.Other, o.Unit, q.Count, 1})
end