-- 涙する悪意

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnOrder)
	ability1.SetCost("Cost")
	ability1.SetGetCosts("GetCosts")
	ability1.SetActivation("Activation")
end

function Cost(check)
	if check then return obj.CanRetire({q.Location, l.PlayerRC, q.Count, 2}) end
	obj.ChooseRetire({q.Location, l.PlayerRC, q.Count, 2})
end

function GetCosts()
	return p.Retire, 2
end

function Activation()
	obj.Draw(1)
	obj.AddToSoul({q.Other, o.ThisFieldID})
	obj.CounterCharge(1)
end