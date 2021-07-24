-- Prayers That Will Reach Someday

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Soul, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.UnitType, u.overDress, q.Count, 1
	elseif n == 3 then
		return q.Location, l.PlayerVC, q.Location, l.PlayerRC, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnBlitzOrder, t.Order, p.HasPrompt, true, p.IsMandatory, false, p.SB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.CanSB(1) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.Exists(2) then
			return true
		end
	end
	return false
end

function Cost(n)
	if n == 1 then
		obj.SoulBlast(1)
	end
end

function Activate(n)
	if n == 1 then
		if obj.Exists(2) then
			obj.ChooseAddBattleOnlyPower(3, 15000)
		end
	end
	return 0
end