-- 極光戦姫 カフ・スプリング

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Condition()
	return obj.HasPrison() and obj.Exists({q.Location, l.EnemyHand, q.Count, 1})
end

function Activation()
	local imprisoned = obj.EnemyChooseImprison({q.Location, l.EnemyHand, q.Count, 1})
	obj.Store(imprisoned)
	if obj.GetNumberOf({q.Location, l.Stored}) > 0 then
		obj.EnemyDraw(1)
	end
end