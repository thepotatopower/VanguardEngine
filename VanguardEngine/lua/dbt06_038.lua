-- スパークルリジェクター・ドラゴン

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PutOnGC)
	ability1.SetTrigger("OnPlaceTrigger")
	ability1.SetActivation("OnPlace")
end

function OnPlaceTrigger()
	return obj.IsApplicable()
end

function OnPlace()
	obj.Select({q.Location, l.PlayerUnits, q.Other, o.Attacked, q.Count, 1})
	obj.AddCardState({q.Location, l.Selected}, cs.CannotBeHit, p.UntilEndOfBattle)
	if obj.GetNumberOf({q.Location, l.Hand}) >= 2 then
		obj.Discard(1)
	end
end