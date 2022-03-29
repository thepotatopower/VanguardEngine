-- 幸甚の魔法 ナナフル

function RegisterAbilities()
	-- on drive check
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnDriveCheck)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.Exists({q.Location, l.Applicable, q.UnitType, u.Trigger, q.Other, o.Player})
end

function Cost(check)
	if check then return obj.CanAddToSoul({q.Location, l.Hand, q.Count, 1}) end
	obj.ChooseAddToSoul({q.Location, l.Hand, q.Count, 1})
end

function Activation()
	obj.Draw(1)
end