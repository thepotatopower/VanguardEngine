-- 極光戦姫 トゥーラ・ブーケンビリア

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("Trigger")
	ability1.SetProperty(p.NotMandatory)
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Condition()
	return obj.Exists({q.Location, l.PlayerPrisoners})
end

function Activation()
	obj.Select({q.Location, l.PlayerPrisoners, q.Count, 1})
	local called = obj.SuperiorCallToSpecificCircle({q.Location, l.Selected}, FL.EnemyCircle, FL.OpenCircle)
	obj.Store(called)
	if obj.GetNumberOf({q.Location, l.Stored}) > 0 then
		obj.ChooseImprison({q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1})
	end
end