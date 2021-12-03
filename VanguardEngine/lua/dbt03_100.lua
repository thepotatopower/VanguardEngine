-- 感得の魔法 キキッチュ

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
	ability1.SetProperty(p.NotMandatory)
end

function Trigger()
	return obj.IsApplicable()
end

function Condition()
	return obj.Exists({q.Location, l.Hand, q.Trigger, tt.Critical, q.Trigger, tt.Front})
end

function Activation()
	obj.Select({q.Location, l.Hand, q.Trigger, tt.Critical, q.Trigger, tt.Front, q.Count, 1, q.Min, 0})
	obj.Reveal({q.Location, l.Selected})
	local sent = obj.SendToBottom({q.Location, l.Selected})
	obj.Store(sent)
	if obj.GetNumberOf({q.Location, l.Stored}) > 0 and obj.CanSB(1) and obj.YesNo(obj.GetDescription(2)) then
		obj.SoulBlast(1)
		obj.Draw(1)
	end
end