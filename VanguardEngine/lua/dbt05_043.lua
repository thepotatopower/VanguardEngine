-- フレイミング・ポニー

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnACT)
	ability1.SetLocation(l.Soul)
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Cost(check)
	if check then return obj.CanBind({q.Other, o.This}) end
	obj.Bind({q.Other, o.This})
end

function Activation()
	obj.SoulCharge(2)
end