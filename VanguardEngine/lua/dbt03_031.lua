-- 襲獲祭

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnOrder)
	ability1.SetActivation("OrderActivate")
end

function OrderActivate()
	obj.Select({q.Location, l.PlayerVC, q.Count, 1})
	if obj.GetNumberOf({q.Location, l.Selected}) > 0 then
		local id = obj.GetID({q.Location, l.Selected})
		local tempAbility = GiveAbility(GetID(), id)
		tempAbility.SetDescription(2)
		tempAbility.SetTiming(a.OnRetire)
		tempAbility.SetLocation(l.VC)
		tempAbility.SetResetTarget(id)
		tempAbility.SetResetTiming(p.UntilEndOfTurn)
		tempAbility.SetTrigger("TempTrigger")
		tempAbility.SetActivation("TempActivation")
	end
end

function TempTrigger()
	return obj.PlayerMainPhase() and obj.Exists({q.Location, l.Applicable, q.Other, o.Enemy})
end

function TempActivation()
	obj.Draw(1)
end