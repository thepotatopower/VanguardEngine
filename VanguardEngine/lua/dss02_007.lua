-- グラビディア・ペテルス

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttackHits)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsBooster()
end

function Condition()
	return obj.Exists({q.Location, l.Drop, q.Other, o.Meteorite})
end

function Activation()
	obj.Store(obj.ChooseSendToBottom({q.Location, l.Drop, q.Other, o.Meteorite, q.Count, 2, q.Min, 0}))
	if obj.GetNumberOf({q.Location, l.Stored}) == 2 then
		obj.Retire({q.Other, o.ThisFieldID})
		obj.Draw(1)
	end
end