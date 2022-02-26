-- デザイアデビル ヤーダ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttack)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.Exists({q.Location, l.PlayerVC, q.Other, o.Attacking})
end

function Condition()
	return obj.Exists({q.Location, l.PlayerUnits, q.Other, o.Standing, q.Count, 3})
end

function Cost(check)
	if check then return obj.CanRetire({q.Other, o.ThisFieldID}) end
	obj.Retire({q.Other, o.ThisFieldID})
end

function Activation()
	obj.SoulCharge(2)
	obj.Select({q.Location, l.Soul, q.Name, obj.GetName({q.Location, l.PlayerVC}), q.Count, 1})
	obj.SendToTop({q.Location, l.Selected})
end