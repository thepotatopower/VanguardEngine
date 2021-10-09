-- Vibrant Symphony

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerVC, q.Name, "Archangel of Twin Wings, Alestiel", q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerUnits, q.Other, o.Attacked, q.Count, 1
	elseif n == 3 then
		return q.Location, l.PlayerRC, q.Other, o.OddGrade
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnBlitzOrder, p.HasPrompt
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.Exists(1) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.ChooseAddBattleOnlyPower(2, obj.GetNumberOf(3) * 5000)
		obj.AddUntilEndOfTurnPlayerState(ps.RetiredEvenGradeUnitsCanBeAddedToSoul)
	end
	return 0
end