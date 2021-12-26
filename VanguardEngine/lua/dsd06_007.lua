-- 封焔竜 シルンガ

function RegisterAbilities()
	-- on attack
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttack)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsBooster()
end

function Cost(check)
	if check then return obj.CanSpecificDiscard({q.Location, l.Hand, q.Other, o.Arms, q.Count, 1}) end
	obj.SpecificDiscard({q.Location, l.Hand, q.Other, o.Arms, q.Count, 1})
end

function Activation()
	obj.Draw(1)
end