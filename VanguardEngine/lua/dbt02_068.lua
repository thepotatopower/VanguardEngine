-- Prayers That Will Reach Someday

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.UnitType, u.overDress, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerVC, q.Location, l.PlayerRC, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnBlitzOrder, p.HasPrompt, p.SB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		return true
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.Exists(1) then
			return true
		end
	end
	return false
end

function Activate(n)
	if n == 1 then
		if obj.Exists(1) then
			obj.ChooseAddBattleOnlyPower(2, 15000)
		end
	end
	return 0
end